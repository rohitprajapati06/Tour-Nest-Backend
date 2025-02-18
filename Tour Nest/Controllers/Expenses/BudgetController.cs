using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TourNest.Models;
using TourNest.Models.TravelExpenses.UserBudget;

namespace TourNest.Controllers.Expenses;


[ApiController]
[Route("api/[controller]")]
public class BudgetController : ControllerBase
{
    private readonly TourNestContext _context;

    public BudgetController(TourNestContext context)
    {
        _context = context;
    }


    [Authorize]
    [HttpPost("CreateExpense")]
    public async Task<IActionResult> AddExpense([FromBody] UserExpenseView expenseView)
    {
        // Validate input
        if (expenseView == null || string.IsNullOrWhiteSpace(expenseView.TripName) || expenseView.Cost <= 0)
        {
            return BadRequest(new { message = "Invalid input. Please provide valid expense details." });
        }

        try
        {
            // Extract UserId from JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User authentication failed. Please log in again." });
            }

            // Ensure UserId is a valid Guid
            if (!Guid.TryParse(userId, out Guid parsedUserId))
            {
                return Unauthorized(new { message = "Invalid User ID." });
            }

            // Map UserExpenseView to UserExpense
            var expense = new UserExpense
            {
                ExpenseId = Guid.NewGuid(),
                UserId = parsedUserId,
                TripName = expenseView.TripName,
                Cost = expenseView.Cost,
                Date = expenseView.Date,
                Category = expenseView.Category,
                Notes = expenseView.Notes
            };

            // Save to database
            await _context.UserExpenses.AddAsync(expense);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Expense added successfully", expense });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while saving the expense.", error = ex.Message });
        }
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetExpense()
    {
        // Extract userId from JWT token
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString))
        {
            return BadRequest(new { message = "User ID cannot be null or empty." });
        }

        // Convert the extracted userId string to Guid
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return BadRequest(new { message = "Invalid User ID format." });
        }

        // Query the database for all expenses of the user
        var userExpenses = await _context.UserExpenses
                                          .Where(expense => expense.UserId == userId)
                                          .ToListAsync();

        if (userExpenses == null || !userExpenses.Any())
        {
            return NotFound(new { message = "No expenses found for the provided user ID." });
        }

        return Ok(userExpenses);
    }





    [Authorize]
    [HttpDelete("{tripid:guid}")]
    public async Task<IActionResult> DeleteExpense(Guid tripid) { 
       var data = await _context.UserExpenses.FindAsync(tripid);
        if (data == null) { 
            return NotFound(new { message = "Now User Found" });
        }
         _context.UserExpenses.Remove(data);
         await _context.SaveChangesAsync(); 
         return Ok(new {message = "Expense Deleted"});
    }
}

