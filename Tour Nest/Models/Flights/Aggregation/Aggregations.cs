using System.Text.Json.Serialization;

namespace TourNest.Models.Flights.Aggregation
{
    public class Aggregations
    {
        public int TotalCount { get; set; }
        public int FilteredTotalCount { get; set; }
        public List<Stop> Stops { get; set; }
        public List<Airline> Airlines { get; set; }
        public List<FlightTimes> FlightTimes { get; set; }
        public List<Duration> Duration { get; set; }
    }
}
