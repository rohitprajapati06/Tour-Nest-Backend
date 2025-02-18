namespace TourNest.Models.Flights.Aggregation
{
    public class FlightTimes
    {
        public List<TimeRange> Arrival { get; set; }
        public List<TimeRange> Departure { get; set; }
    }
}
