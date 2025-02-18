using System.Text.Json.Serialization;
using TourNest.Models.Flights.Aggregation;

namespace TourNest.Models.Flights.Search_Flights.Segments
{
    public class FlightData
    {
        [JsonIgnore]
        public Aggregations Aggregation { get; set; }
        public List<FlightOffers> FlightOffers { get; set; }
        public List<BaggagePolicy> BaggagePolicies { get; set; }
    }
}
