namespace TourNest.Models.Flights.Search_Flights
{
    public class FlightDeals
    {
        public string Key { get; set; }
        public string offerToken { get; set; }    

        public Price Price { get; set; }

        public TravellerPrices TravellerPrices { get; set; }
    }
}
