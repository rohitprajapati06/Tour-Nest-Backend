namespace TourNest.Models.TravelExpenses.PreDefined_Budgets
{
    public partial class LuxuryBudget
    {
        public int Id { get; set; }

        public string Location { get; set; } = null!;

        public string Transportation { get; set; } = null!;

        public string Accommodation { get; set; } = null!;

        public string Food { get; set; } = null!;

        public string Attractions { get; set; } = null!;

        public string Miscellaneous { get; set; } = null!;

        public string Total { get; set; } = null!;
    }
}
