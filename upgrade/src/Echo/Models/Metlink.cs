using System;
using Newtonsoft.Json;

namespace Echo.Models
{
    public partial class MetlinkResponse
    {
        [JsonProperty("farezone")]
        public long Farezone { get; set; }

        [JsonProperty("closed")]
        public bool Closed { get; set; }

        [JsonProperty("departures")]
        public Departure[] Departures { get; set; }
    }

    public partial class Departure
    {
        [JsonProperty("stop_id")]
        public string StopId { get; set; }

        [JsonProperty("service_id")]
        public string ServiceId { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("operator")]
        public string Operator { get; set; }

        [JsonProperty("origin")]
        public Destination Origin { get; set; }

        [JsonProperty("destination")]
        public Destination Destination { get; set; }

        [JsonProperty("delay")]
        public string Delay { get; set; }

        [JsonProperty("vehicle_id")]
        public long? VehicleId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("arrival")]
        public TrainTime ArrivalTime { get; set; }

        [JsonProperty("departure")]
        public TrainTime DepartureTime { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("monitored")]
        public bool Monitored { get; set; }

        [JsonProperty("wheelchair_accessible")]
        public bool WheelchairAccessible { get; set; }

        [JsonProperty("trip_id")]
        public bool TripId { get; set; }
    }

    public partial class TrainTime
    {
        [JsonProperty("aimed")]
        public DateTime Aimed { get; set; }

        [JsonProperty("expected")]
        public DateTime? Expected { get; set; }
    }

    public partial class Destination
    {
        [JsonProperty("stop_id")]
        public string StopId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Stop
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("stop_id")]
        public string StopId { get; set; }

        [JsonProperty("stop_code")]
        public string StopCode { get; set; }

        [JsonProperty("stop_name")]
        public string StopName { get; set; }

        [JsonProperty("stop_desc")]
        public string StopDesc { get; set; }

        [JsonProperty("zone_id")]
        public string ZoneId { get; set; }

        [JsonProperty("stop_lat")]
        public double StopLat { get; set; }

        [JsonProperty("stop_lon")]
        public double StopLon { get; set; }

        [JsonProperty("location_type")]
        public string LocationType { get; set; }

        [JsonProperty("parent_station")]
        public string ParentStation { get; set; }

        [JsonProperty("stop_url")]
        public string StopUrl { get; set; }

        [JsonProperty("stop_timezone")]
        public string StopTimezone { get; set; }
    }
}
