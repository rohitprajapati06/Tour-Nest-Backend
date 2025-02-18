using TourNest.Models.Flights.Search_Flights.Segments;

namespace TourNest.Models.Flights.Search_Flights
{
    public class FlightResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public long Timestamp { get; set; }
        public Data Data { get; set; }
    }
}
