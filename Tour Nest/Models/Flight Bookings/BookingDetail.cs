using System.Text.Json.Serialization;
using TourNest.Models.Payment_Details;
using TourNest.Models.UserProfile;

namespace TourNest.Models.Flight_Bookings
{
    public partial class BookingDetail
    {
        public Guid UserId { get; set; }

        public Guid BookingId { get; set; }

        public DateTime TravellerDate { get; set; }

        public string DepartureAirport { get; set; } = null!;

        public string ArrivalAirport { get; set; } = null!;

        public string CabinClass { get; set; } = null!;

        public string PlaneNumber { get; set; } = null!;

        public string Status { get; set; } = null!;

        public int Passenger { get; set; }

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        [JsonIgnore]
        public virtual ICollection<TravellerDetail> TravellerDetails { get; set; } = new List<TravellerDetail>();
        [JsonIgnore]
        public virtual User User { get; set; } = null!;
    }

}
