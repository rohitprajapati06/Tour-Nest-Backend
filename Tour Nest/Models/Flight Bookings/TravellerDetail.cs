using System.Text.Json.Serialization;

namespace TourNest.Models.Flight_Bookings
{
    public class TravellerDetail
    {
        public Guid BookingId { get; set; }

        public Guid TravellerId { get; set; }

        public string Name { get; set; } = null!;

        public int Age { get; set; }

        [JsonIgnore]
        public virtual BookingDetail Booking { get; set; } = null!;
    }
}
