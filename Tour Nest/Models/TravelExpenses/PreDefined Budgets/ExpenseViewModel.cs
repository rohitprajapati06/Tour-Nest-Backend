using static Azure.Core.HttpHeader;

namespace TourNest.Models.TravelExpenses.PreDefined_Budgets
{
    public class ExpenseViewModel
    {
        public IEnumerable<NormalBudget> NormalBudgets { get; set; }
        public IEnumerable<MidRangeBudget> MidRangeBudgets { get; set; }

        public IEnumerable<LuxuryBudget> LuxuryBudgets { get; set; }
    }
}
