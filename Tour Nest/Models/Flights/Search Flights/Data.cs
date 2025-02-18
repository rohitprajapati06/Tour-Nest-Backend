using System.Text.Json.Serialization;
using TourNest.Models.Flights.Aggregation;

namespace TourNest.Models.Flights.Search_Flights
{
    public class Data
    {
        public Aggregations Aggregation { get; set; }
        public List<FlightOffers> FlightOffers { get; set; }
        public List<BaggagePolicy> BaggagePolicies { get; set; }
    }
}
