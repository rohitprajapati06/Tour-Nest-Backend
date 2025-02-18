using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourNest.Models;
using TourNest.Models.TravelExpenses.PreDefined_Budgets;

namespace TourNest.Controllers.Expenses
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreDefinedExpenseController : ControllerBase
    {
        private readonly TourNestContext context;

        public PreDefinedExpenseController(TourNestContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetBudget()
        {
            var normalbudget = await context.NormalBudgets.ToListAsync();
            var midrangebudget = await context.MidRangeBudgets.ToListAsync();
            var luxurybudget = await context.LuxuryBudgets.ToListAsync();

            var expensemodel = new ExpenseViewModel
            {
                NormalBudgets = normalbudget,
                MidRangeBudgets = midrangebudget,
                LuxuryBudgets = luxurybudget,
            };

            return Ok(expensemodel);
        }
    }
}
