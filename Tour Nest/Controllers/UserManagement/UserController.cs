using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TourNest.Models;
using TourNest.Models.UserProfile.UserDetails;
using TourNest.Services.Mail;
using TourNest.Services.Otp;

namespace TourNest.Controllers.UserManagement;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly TourNestContext context;
    private readonly IEmailServices emailServices;
    private readonly IOtpServices otpServices;

    private static readonly Dictionary<string, int> OtpStorage = new();

    public UserController(TourNestContext context, IEmailServices emailServices, IOtpServices otpServices)
    {
        this.context = context;
        this.emailServices = emailServices;
        this.otpServices = otpServices;
    }


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserById()
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

        // Query the database for the user
        var user = await context.Users
                                .Where(u => u.UserId == userId)
                                .Select(u => new
                                {
                                    u.Email,
                                    u.ProfilePhoto,
                                    u.Username
                                })
                                .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound(new { message = "User not found with the provided ID." });
        }

        return Ok(user);
    }


    [Authorize]
    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp()
    {
        // Extract user ID from claims
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        var user = await context.Users.FindAsync(Guid.Parse(userId));
        if (user == null)
            return NotFound("User not found.");

        // Generate and store OTP
        var otp = await otpServices.GenerateOtpAsync();
        OtpStorage[userId] = otp;

        try
        {
            await emailServices.SendEmailAsync(user.Email, "Your OTP", $"Your OTP is: {otp}");
            return Ok(new { Message = "OTP sent successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Failed to send OTP: {ex.Message}");
        }
    }
    


    [Authorize]
    [HttpPatch("update-email")]
    public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailDto updateEmailDto, [FromQuery] int otp)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Extract user ID from claims
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        if (!OtpStorage.ContainsKey(userId) || OtpStorage[userId] != otp)
            return BadRequest("Invalid or expired OTP.");

        var user = await context.Users.FindAsync(Guid.Parse(userId));
        if (user == null)
            return NotFound("User not found.");

        user.Email = updateEmailDto.Email;

        try
        {
            await context.SaveChangesAsync();
            OtpStorage.Remove(userId); // Remove OTP after successful update
            return Ok(new { Message = "Email updated successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }



    [Authorize]
    [HttpPatch("update-password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto, [FromQuery] int otp)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Extract user ID from claims
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        if (!OtpStorage.ContainsKey(userId) || OtpStorage[userId] != otp)
            return BadRequest("Invalid or expired OTP.");

        var user = await context.Users.FindAsync(Guid.Parse(userId));
        if (user == null)
            return NotFound("User not found.");

        user.Password = BCrypt.Net.BCrypt.HashPassword(updatePasswordDto.Password);

        try
        {
            await context.SaveChangesAsync();
            OtpStorage.Remove(userId); // Remove OTP after successful update
            return Ok(new { Message = "Password updated successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [Authorize]
    [HttpPatch("update-username")]
    public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameDto updateUsernameDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Extract user ID from claims
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        var user = await context.Users.FindAsync(Guid.Parse(userId));
        if (user == null)
            return NotFound("User not found.");

        user.Username = updateUsernameDto.Username;

        try
        {
            await context.SaveChangesAsync();
            return Ok(new { Message = "Username updated successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}
