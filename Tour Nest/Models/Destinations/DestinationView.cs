
namespace TourNest.Models.Destinations
{
    public class DestinationView
    {
        public IEnumerable<TopPlace> TopPlaces { get; set; }
        public IEnumerable<TopDestinationsInIndium> TopDestinationsInIndia { get; set; }
        public IEnumerable<TopTemple> TopTemples { get; set; }
    }
}
