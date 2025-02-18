using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RazorpayIntegration.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TourNest.Models;
using TourNest.Models.Payment_Details;

namespace TourNest.Controllers.Payments
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Ensures only authenticated users can access
    public class PaymentController : ControllerBase
    {
        private readonly RazorpayService _razorpayService;
        private readonly TourNestContext _context;

        public PaymentController(RazorpayService razorpayService, TourNestContext context)
        {
            _razorpayService = razorpayService;
            _context = context;
        }

        // POST: api/Payment/create-order
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest orderRequest)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid parsedUserId))
            {
                return Unauthorized("Invalid or missing User ID in token.");
            }

            try
            {
                // Create Razorpay order
                var razorpayOrder = await _razorpayService.CreateOrderAsync(orderRequest.Amount, orderRequest.Currency, orderRequest.ReceiptId);

                // Create payment record with PaymentId as null initially
                var paymentRecord = new Payment
                {
                    OrderId = razorpayOrder["id"].ToString(), // Store Razorpay Order ID
                    UserId = parsedUserId,
                    BookingId = orderRequest.BookingId,
                    TimeStamp = DateTime.UtcNow,
                    Amount = orderRequest.Amount.ToString(),
                    PaymentStatus = "Created",
                    PaymentId = null // Initially, PaymentId is null
                };

                _context.Payments.Add(paymentRecord);
                await _context.SaveChangesAsync(); // Save the record

                return Ok(new { orderId = razorpayOrder["id"].ToString() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while creating the order: {ex.Message}" });
            }
        }

        // POST: api/Payment/update-payment-status
        [HttpPost("update-payment-status")]
        public async Task<IActionResult> UpdatePaymentStatus([FromBody] PaymentStatusUpdateRequest request)
        {
            if (string.IsNullOrEmpty(request.OrderId) || string.IsNullOrEmpty(request.PaymentId))
            {
                return BadRequest("Both Order ID and Payment ID are required.");
            }

            try
            {
                // Find payment record by OrderId
                var paymentRecord = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == request.OrderId);
                if (paymentRecord == null)
                {
                    return NotFound("Payment record not found for this Order ID.");
                }

                // Fetch actual payment details from Razorpay
                var paymentDetails = await _razorpayService.FetchPaymentDetailsAsync(request.PaymentId);
                if (paymentDetails == null || string.IsNullOrEmpty(paymentDetails["status"]?.ToString()))
                {
                    return BadRequest("Invalid payment details from Razorpay.");
                }

                // Update PaymentId (now available) and PaymentStatus
                paymentRecord.PaymentId = request.PaymentId; // Now set the actual PaymentId
                paymentRecord.PaymentStatus = paymentDetails["status"].ToString();

                await _context.SaveChangesAsync();

                return Ok(new { message = "Payment updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while updating the payment status: {ex.Message}" });
            }
        }

        // GET: api/Payment/payment-details/{paymentId}
        [HttpGet("payment-details/{paymentId}")]
        public async Task<IActionResult> GetPaymentDetails(string paymentId)
        {
            try
            {
                var paymentDetails = await _razorpayService.FetchPaymentDetailsAsync(paymentId);
                var paymentRecord = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);

                if (paymentRecord != null)
                {
                    paymentRecord.PaymentStatus = paymentDetails["status"].ToString();
                    await _context.SaveChangesAsync();
                }

                return Ok(paymentDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while fetching payment details: {ex.Message}" });
            }
        }
    }

    // Request class to handle payment status update from frontend
    public class PaymentStatusUpdateRequest
    {
        public string OrderId { get; set; } = null!;
        public string PaymentId { get; set; } = null!;
    }

    public class OrderRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "INR";
        public string ReceiptId { get; set; }
        public Guid BookingId { get; set; }
    }
}
