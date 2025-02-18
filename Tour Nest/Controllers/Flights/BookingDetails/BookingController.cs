using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TourNest.Models;
using TourNest.Models.Flight_Bookings;
using TourNest.Models.Payment_Details;

namespace TourNest.Controllers.Flights.BookingDetails
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Ensures the user is authenticated
    public class BookingController : ControllerBase
    {
        private readonly TourNestContext _context;

        public BookingController(TourNestContext context)
        {
            _context = context;
        }

        // POST: api/Booking
        [HttpPost]
        public async Task<IActionResult> PostBooking([FromBody] BookingRequestDto bookingRequest)
        {
            if (bookingRequest == null)
            {
                return BadRequest("Booking data is invalid.");
            }

            try
            {
                // Extract UserId from JWT claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized("User ID is missing in token.");
                }

                // Generate a new BookingId
                var bookingId = Guid.NewGuid();

                // Create a new BookingDetail from the request DTO
                var bookingDetail = new BookingDetail
                {
                    BookingId = bookingId,
                    UserId = Guid.Parse(userIdClaim.Value), // Convert extracted UserId to GUID
                    TravellerDate = bookingRequest.TravellerDate,
                    DepartureAirport = bookingRequest.DepartureAirport,
                    ArrivalAirport = bookingRequest.ArrivalAirport,
                    CabinClass = bookingRequest.CabinClass,
                    PlaneNumber = bookingRequest.PlaneNumber,
                    Status = bookingRequest.Status,
                    Passenger = bookingRequest.Passenger,
                    Email = bookingRequest.Email,
                    Phone = bookingRequest.Phone
                };

                // Add the new booking detail to the database
                await _context.BookingDetails.AddAsync(bookingDetail);
                await _context.SaveChangesAsync();

                // Return the created booking details
                return CreatedAtAction(nameof(PostBooking), new { id = bookingDetail.BookingId }, new { bookingId = bookingDetail.BookingId });
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("TravellerDetails")]
        public async Task<IActionResult> PostTravellerDetails([FromBody] TravellerDetailDTO travellerDetailDto)
        {
            if (travellerDetailDto == null)
            {
                return BadRequest("Traveller data is invalid.");
            }

            try
            {
                // Validate if the booking exists
                var booking = await _context.BookingDetails.FindAsync(travellerDetailDto.BookingId);
                if (booking == null)
                {
                    return NotFound("Booking not found.");
                }

                // Create a new TravellerDetail entity
                var newTraveller = new TravellerDetail
                {
                    TravellerId = Guid.NewGuid(), // Generate a new Traveller ID
                    BookingId = travellerDetailDto.BookingId,
                    Name = travellerDetailDto.Name,
                    Age = travellerDetailDto.Age
                };

                // Add the traveler detail to the database
                await _context.TravellerDetails.AddAsync(newTraveller);
                await _context.SaveChangesAsync();

                // Return the created traveler details using DTO
                return CreatedAtAction(nameof(PostTravellerDetails), new { id = newTraveller.TravellerId }, travellerDetailDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetBookingsByUser()
        {
            // Extract UserId from JWT claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized("Invalid user token.");
            }

            // Fetch bookings with TravellerDetails and Payments, sorted by the latest booking date (TravellerDate)
            var bookings = await _context.BookingDetails
                .Where(b => b.UserId == userId)
                .Include(b => b.TravellerDetails)
                .Include(b => b.Payments)
                .Select(b => new BookingDetailDTO
                {
                    UserId = b.UserId,
                    BookingId = b.BookingId,
                    TravellerDate = b.TravellerDate,
                    DepartureAirport = b.DepartureAirport,
                    ArrivalAirport = b.ArrivalAirport,
                    CabinClass = b.CabinClass,
                    PlaneNumber = b.PlaneNumber,
                    Status = b.Status,
                    Passenger = b.Passenger,
                    Email = b.Email,
                    Phone = b.Phone,
                    TravellerDetails = b.TravellerDetails.Select(t => new TravellerDetailDTO
                    {
                        BookingId = t.BookingId,
                        TravellerId = t.TravellerId,
                        Name = t.Name,
                        Age = t.Age
                    }).ToList(),
                    Payments = b.Payments.Select(p => new PaymentDTO
                    {
                        PaymentId = p.PaymentId,
                        UserId = p.UserId,
                        BookingId = p.BookingId,
                        TimeStamp = p.TimeStamp,
                        Amount = p.Amount,
                        PaymentStatus = p.PaymentStatus,
                        OrderId = p.OrderId
                    }).ToList()
                })
                .OrderByDescending(b => b.TravellerDate) // Sorting the bookings by the latest booking date (TravellerDate)
                .ToListAsync();

            if (bookings == null || bookings.Count == 0)
            {
                return NotFound("No bookings found for the user.");
            }

            return Ok(bookings);
        }


    }
}
