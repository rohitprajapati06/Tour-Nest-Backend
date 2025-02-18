using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourNest.Models;
using TourNest.Models.UserProfile;
using TourNest.Services.Mail;
using TourNest.Services.MemoryCache;
using TourNest.Services.Otp;
using TourNest.Services.JWT;
using Microsoft.Extensions.Configuration;
using TourNest.Models.JWT_Refresh_Token;

namespace TourNest.Controllers.UserManagement;

[Route("api/[controller]")]
[ApiController]
public class AuthController(TourNestContext context, IMemoryCacheService memoryCacheService, IEmailServices services, IOtpServices otpServices , IConfiguration configuration) : ControllerBase
{
    private readonly TourNestContext context = context;
    private readonly IMemoryCacheService _memoryCacheService = memoryCacheService;
    private readonly IEmailServices services = services;
    private readonly IOtpServices otpServices = otpServices;
    private readonly IConfiguration _configuration = configuration;



    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO user)
    {
        if (ModelState.IsValid)
        {
            var userExist = await context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (userExist != null)
            {
                return BadRequest(new { message = "Email already registered." });
            }

            if (user.Password != user.ConfirmPassword)
            {
                return BadRequest(new { message = "Passwords do not match." });
            }

            // Generate OTP and send it via email
            int otp = await otpServices.GenerateOtpAsync();
            var email = user.Email;
            var subject = "Confirm Your Email";
            var message = $"Your six-digit OTP for Tournest is {otp}";

            try
            {
                await services.SendEmailAsync(email, subject, message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            // Hash the password
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            // Create Cached Data
            var cachedData = new CachedUserData
            {
                Otp = otp,
                User = user
            };

            // Store in memory cache
            _memoryCacheService.Set(user.Email, cachedData, TimeSpan.FromMinutes(10));

            return Ok(new { message = "OTP sent to your email. Please verify to complete registration." });
        }
        else
        {
            return BadRequest(new { message = "Enter Details Properly" });
        }
    }




    [HttpPost("VerifyOtp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDTO dto)
    {
        var cachedData = _memoryCacheService.Get<CachedUserData>(dto.Email);

        if (cachedData == null)
        {
            return BadRequest(new { message = "Invalid OTP or OTP expired." });
        }

        if (cachedData.Otp != dto.Otp)
        {
            return BadRequest(new { message = "Invalid OTP or OTP expired." });
        }

        // Map RegisterDTO to User and save to database
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = $"{cachedData.User.FirstName} {cachedData.User.LastName}",
            Email = cachedData.User.Email,
            Password = cachedData.User.Password,
            TimeStamp = DateTime.UtcNow
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Remove cached data
        _memoryCacheService.Remove(dto.Email);

        return Ok(new { message = "Registration completed successfully." });
    }




    [HttpPost("Login")]
    public async Task<IActionResult> Loginuser(LoginDTO login)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == login.Email);
        if (user != null && BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
        {
            var jwtHelper = new JwtHelper(_configuration);
            var tokens = jwtHelper.GenerateTokens(user.UserId.ToString(), user.Username);

            // Save refresh token (In-memory for demo)
            RefreshTokenStore.RefreshTokens[tokens.RefreshToken] = user.Email;

            return Ok(tokens);
        }
        return Unauthorized(new { message = "Invalid email or password." });
    }



    [HttpPost("Refresh")]
    public IActionResult RefreshToken([FromBody] TokenModel model)
    {
        if (!RefreshTokenStore.RefreshTokens.ContainsKey(model.RefreshToken) ||
            RefreshTokenStore.RefreshTokens[model.RefreshToken] != User.Identity.Name)
        {
            return Unauthorized("Invalid refresh token.");
        }

        // Generate new tokens
        var tokenHelper = new JwtHelper(_configuration);
        var tokens = tokenHelper.GenerateTokens("1", User.Identity.Name);

        // Update store
        RefreshTokenStore.RefreshTokens.Remove(model.RefreshToken);
        RefreshTokenStore.RefreshTokens[tokens.RefreshToken] = User.Identity.Name;

        return Ok(tokens);
    }

}
