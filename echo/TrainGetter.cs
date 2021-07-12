using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Echo
{
    public class TrainGetter
    {
        private const string DepartureFormat = "The next departure from {0} heading to {1} leaves in {2} minutes and {3} seconds. ";

        public static async Task<string> Get(string text = "AVA")
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", "iuoMNXQjzC1PjijgMjKkHhYWPb4ZES2UpaYfgsd0");

                var url = GetUrl(text);

                try
                {
                    var response = await client.GetAsync(url);

                    Console.WriteLine("Metlink request status: " + response.StatusCode);

                    if (response.StatusCode == HttpStatusCode.OK && response.Content != null)
                    {
                        var metlinkResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(metlinkResponse);

                        var responseObject = JObject.Parse(metlinkResponse);

                        if (url.EndsWith("WELL"))
                        {
                            return GetWellingtonResponse(responseObject);
                        }
                        else
                        {
                            return GetExternalStationResponse(responseObject);
                        }
                    }
                    else
                    {
                        var errorResponse = "I didn't understand the name of your station. Please try again using the name of a station in the Wellington train network.";
                        return errorResponse;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Metlink request failed: " + ex.Message);
                }
            }

            return string.Empty;
        }

        private static string GetUrl(string text)
        {
            const string url = "https://www.metlink.org.nz/api/v1/StopDepartures/";

            if (string.IsNullOrEmpty(text))
            {
                text = "AVA";
            }

            text = text.Replace(" ", "");

            if (text.Length > 4)
            {
                text = text.Substring(0, 4);
            }

            var encodedString = System.Uri.EscapeUriString(text).ToUpper();

            return string.Format("{0}{1}", url, encodedString);
        }

        private static Dictionary<string, string> GetDestinations()
        {
            return new Dictionary<string, string>()
            {
                {"UPPE", "Upper Hutt"},
                {"TAIT2", "Taita"},
                {"TAIT", "Taita"},
                {"WAIK", "Waikanae"},
                {"PORI2", "Porirua"},
                {"JOHN", "Johnsonville"},
                {"MELL", "Melling"},
                {"WELL", "Wellington"},
            };
        }

        private static string GetWellingtonResponse(JObject responseObject)
        {
            var stop = responseObject["Stop"].Value<string>("Name");

            var responseString = string.Empty;

            var destinations = GetDestinations();

            foreach (var key in destinations.Keys)
            {
                var departure = GetWellingtonDeparture(key, destinations[key], responseObject["Services"]);

                if (departure != null)
                {
                    responseString += string.Format(DepartureFormat, stop, departure.Destination, departure.Minutes, departure.Seconds);
                }
                else if (key != "WELL")
                {
                    responseString += string.Format("No {0} departures listed. ", destinations[key]);
                }
            }

            return responseString;
        }

        private static Departure GetWellingtonDeparture(string destinationKey, string destinationName, IEnumerable<JToken> services)
        {
            foreach (var service in services)
            {
                if (service.Value<string>("DestinationStopID") != destinationKey)
                {
                    continue;
                }

                return new Departure
                {
                    Direction = "Outbound",
                    Destination = destinationName,
                    Minutes = service.Value<int>("DisplayDepartureSeconds") / 60,
                    Seconds = service.Value<int>("DisplayDepartureSeconds") % 60
                };
            }

            return null;
        }

        private static string GetExternalStationResponse(JObject responseObject)
        {
            var stop = responseObject["Stop"].Value<string>("Name");
            var inbound = GetExternalDeparture("Inbound", responseObject["Services"]);
            var outbound = GetExternalDeparture("Outbound", responseObject["Services"]);

            var responseString = "";

            if (!string.IsNullOrWhiteSpace(inbound?.Destination))
            {
                var inboundDestination = GetExternalDestination(inbound.Destination);
                responseString = string.Format(DepartureFormat, stop, inboundDestination, inbound.Minutes, inbound.Seconds);
            }

            if (!string.IsNullOrWhiteSpace(outbound?.Destination))
            {
                var outboundDestination = GetExternalDestination(outbound.Destination);
                responseString += string.Format(DepartureFormat, stop, outboundDestination, outbound.Minutes, outbound.Seconds);
            }

            return responseString;
        }

        private static Departure GetExternalDeparture(string direction, IEnumerable<JToken> services)
        {
            foreach (var service in services)
            {
                if (service.Value<string>("Direction") != direction || service.Value<string>("ServiceID") == "WRL")
                {
                    continue;
                }

                return new Departure
                {
                    Direction = direction,
                    Destination = service.Value<string>("DestinationStopID"),
                    Minutes = service.Value<int>("DisplayDepartureSeconds") / 60,
                    Seconds = service.Value<int>("DisplayDepartureSeconds") % 60
                };
            }

            return null;
        }

        private static string GetExternalDestination(string destination)
        {
            var destinations = GetDestinations();

            var words = destination.Split(' ');

            return destinations[words[0]];
        }
    }

    public class Departure
    {
        public string Direction { get; set; }
        public string Destination { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
    }
}
