using TourNest.Models.Flights.Luggage;

namespace TourNest.Models.Flights.Search_Flights.Segments
{
    public class Segment
    {
        public FlightLocation DepartureAirport { get; set; }
        public FlightLocation ArrivalAirport { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public List<Legs> Legs { get; set; }
        public int TotalTime { get; set; }
        public List<LuggageAllowances> TravellerCheckedLuggage { get; set; }
        public List<LuggageAllowances> TravellerCabinLuggage { get; set; }
    }
}
