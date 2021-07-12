using System.Collections.Generic;

namespace Echo
{
    public static class Constants
    {
        public static class Metlink
        {
            public const string DefaultStationCode = "AVA";

            public const string ApiKeyName = "MetlinkApiKey";
            public const string ApiKeyHeaderName = "x-api-key";

            public static Dictionary<string, string> Destinations = new Dictionary<string, string>()
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

            public static class Routes
            {
                public const string BaseUrl = "https://api.opendata.metlink.org.nz/v1";
                public const string StopPredictions = "/stop-predictions?stop_id=";
                public const string Stops = "/gtfs/stops";
            }
    }

        public static class Response
        {
            public const string DepartureFormat = "The next departure from {0} heading to {1} leaves in {2} minutes and {3} seconds. ";
        }
    }
}
