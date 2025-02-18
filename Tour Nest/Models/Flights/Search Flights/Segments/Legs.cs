namespace TourNest.Models.Flights.Search_Flights.Segments
{
    public class Legs
    {
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string CabinClass { get; set; }
        public FlightInfo FlightInfo { get; set; }
        public List<CarriersData> CarriersData { get; set; }
        public int TotalTime { get; set; }



    }
}
