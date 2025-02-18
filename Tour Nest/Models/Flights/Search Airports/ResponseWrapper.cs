
namespace TourNest.Models.Flights.Search_Flights
{
    public class ResponseWrapper
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public long Timestamp { get; set; }
        public List<FlightLocation> Data { get; set; }
    }
}
