namespace TourNest.Models.Payment_Details
{
    public class PaymentDTO
    {
        public string PaymentId { get; set; } = null!;

        public Guid UserId { get; set; }

        public Guid BookingId { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Amount { get; set; } = null!;

        public string PaymentStatus { get; set; } = null!;

        public string OrderId { get; set; } = null!;
    }
}
