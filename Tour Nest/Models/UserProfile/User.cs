using TourNest.Models.BloggingPlatform;
using TourNest.Models.Flight_Bookings;
using TourNest.Models.Payment_Details;
using TourNest.Models.Rating_and_Reviews;
using TourNest.Models.TravelExpenses.UserBudget;

namespace TourNest.Models.UserProfile;

public partial class User
{
    internal string? Id;

    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime TimeStamp { get; set; }

    public string? ProfilePhoto { get; set; }

    public virtual ICollection<Blogging> Bloggings { get; set; } = new List<Blogging>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual ICollection<UserExpense> UserExpenses { get; set; } = new List<UserExpense>();
}
