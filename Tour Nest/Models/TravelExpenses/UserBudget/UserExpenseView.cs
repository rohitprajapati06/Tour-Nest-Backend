using TourNest.Models.UserProfile;

namespace TourNest.Models.TravelExpenses.UserBudget
{
    public partial class UserExpenseView
    {
        public Guid ExpenseId { get; set; }

        public Guid UserId { get; set; }

        public string TripName { get; set; } = null!;

        public decimal Cost { get; set; }

        public DateOnly Date { get; set; }

        public string Category { get; set; } = null!;

        public string? Notes { get; set; }
    }
}
