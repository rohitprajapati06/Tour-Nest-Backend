using TourNest.Models.Flights.Search_Flights.Segments;

namespace TourNest.Models.Flights.Search_Flights
{
    public class FlightOffers
    {
        public string Token { get; set; }
        public List<Segment> Segments { get; set;}
        public PriceBreakdown PriceBreakdown { get; set; }
        public List<TravellerPrices> TravellerPrices { get; set; }


    }
}
