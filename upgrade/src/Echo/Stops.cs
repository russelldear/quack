using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Echo.Models;
using Newtonsoft.Json;

namespace Echo
{
    public class Stops
    {
        private const string _url = "https://api.opendata.metlink.org.nz/v1/gtfs/stops";

        private static Dictionary<string, string> _current;
        public static async Task<Dictionary<string, string>> Current()
        {
            if (_current == null)
            {
                _current = await GetStops();
            }

            return _current;
        }

        private static async Task<Dictionary<string, string>> GetStops()
        {
            var result = new Dictionary<string, string>();

            var apiKey = Environment.GetEnvironmentVariable("MetlinkApiKey");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);

                try
                {
                    var response = await client.GetAsync(_url);

                    Console.WriteLine("Stop retrieval status: " + response.StatusCode);

                    var responseString = await response.Content.ReadAsStringAsync();

                    var stops = JsonConvert.DeserializeObject<List<Stop>>(responseString);

                    return new Dictionary<string, string>(stops.Select(s => new KeyValuePair<string, string>(s.StopId, s.StopName)));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Stop retrieval request failed: " + ex.Message);
                    return null;
                }
            }
        }
    }
}
