using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Echo.Models;
using Newtonsoft.Json;
using static Echo.Constants.Metlink;
using static Echo.Constants.Metlink.Routes;
using static Echo.Constants.Response;

namespace Echo
{
    public class TrainGetter
    {
        public static async Task<string> Get(string text = DefaultStationCode)
        {
            var apiKey = Environment.GetEnvironmentVariable(ApiKeyName);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add(ApiKeyHeaderName, apiKey);

                var url = BuildUrl(text);

                try
                {
                    var response = await client.GetAsync(url);

                    Console.WriteLine("Metlink request status: " + response.StatusCode);

                    if (response.StatusCode == HttpStatusCode.OK && response.Content != null)
                    {
                        var metlinkResponseString = await response.Content.ReadAsStringAsync();

                        Console.WriteLine(metlinkResponseString);

                        var metlinkResponse = JsonConvert.DeserializeObject<MetlinkResponse>(metlinkResponseString);

                        if (url.EndsWith("WELL"))
                        {
                            return GetWellingtonResponse(metlinkResponse);
                        }
                        else
                        {
                            return await GetExternalStationResponse(metlinkResponse);
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

        private static string GetWellingtonResponse(MetlinkResponse metlinkResponse)
        {
            var responseString = string.Empty;

            foreach (var key in Destinations.Keys)
            {
                var firstDeparture = metlinkResponse.Departures.FirstOrDefault(d => d.Destination.StopId == key);

                if (firstDeparture != null)
                {
                    var departureResponse = new DepartureResponse
                    {
                        Direction = "Outbound",
                        Destination = Destinations[key],
                        Minutes = firstDeparture.DepartureTime.GetSecondsFromNow() / 60,
                        Seconds = firstDeparture.DepartureTime.GetSecondsFromNow() % 60
                    };

                    responseString += string.Format(DepartureFormat, "Wellington", Destinations[key], departureResponse.Seconds);
                }
                else if (key != "WELL")
                {
                    responseString += string.Format("No {0} departures listed. ", Destinations[key]);
                }
            }

            return responseString;
        }

        private static async Task<string> GetExternalStationResponse(MetlinkResponse metlinkResponse)
        {
            var responseString = "";

            responseString += await BuildResponseString("inbound", metlinkResponse.Departures.ToList());

            responseString += await BuildResponseString("outbound", metlinkResponse.Departures.ToList());

            return string.IsNullOrWhiteSpace(responseString) ? "No departures found" : responseString;
        }

        private static async Task<string> BuildResponseString(string direction, List<Departure> orderedDepartures)
        {
            var stops = await Stops.Current();

            var stop = stops[orderedDepartures.First().StopId];

            var nextDeparture = orderedDepartures.FirstOrDefault(d => d.Direction == direction && d.ServiceId != "WRL");

            if (nextDeparture != null)
            {
                var departureResponse = new DepartureResponse
                {
                    Direction = direction,
                    Destination = stops[nextDeparture.Destination.StopId],
                    Minutes = nextDeparture.DepartureTime.GetSecondsFromNow() / 60,
                    Seconds = nextDeparture.DepartureTime.GetSecondsFromNow() % 60
                };

                return string.Format(DepartureFormat, stop, stops[nextDeparture.Destination.StopId], departureResponse.Minutes, departureResponse.Seconds);
            }

            return string.Empty;
        }

        private static string BuildUrl(string text)
        {
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

            return $"{BaseUrl}{StopPredictions}{encodedString}";
        }
    }
}
