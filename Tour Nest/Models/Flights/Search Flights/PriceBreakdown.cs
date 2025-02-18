namespace TourNest.Models.Flights.Search_Flights
{
    public class PriceBreakdown
    {
        public PriceDetail Total { get; set; }

        public PriceDetail BaseFare { get; set; }

        public PriceDetail Tax { get;set; }
    }
}
