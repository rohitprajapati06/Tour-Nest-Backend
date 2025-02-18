using System.Text.Json.Serialization;
using TourNest.Models.Flight_Bookings;
using TourNest.Models.UserProfile;

namespace TourNest.Models.Payment_Details;

public partial class Payment
{
    public string PaymentId { get; set; } = null!;

    public Guid UserId { get; set; }

    public Guid BookingId { get; set; }

    public DateTime TimeStamp { get; set; }

    public string Amount { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;

    public string OrderId { get; set; } = null!;
    [JsonIgnore]
    public virtual BookingDetail Booking { get; set; } = null!;
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}