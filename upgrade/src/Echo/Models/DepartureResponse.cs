using System;
namespace Echo.Models
{
    public class DepartureResponse
    {
        public string Direction { get; set; }
        public string Destination { get; set; }
        public double Minutes { get; set; }
        public double Seconds { get; set; }
    }
}
