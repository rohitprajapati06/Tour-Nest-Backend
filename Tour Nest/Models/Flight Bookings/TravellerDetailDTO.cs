namespace TourNest.Models.Flight_Bookings
{
    public class TravellerDetailDTO
    {
        public Guid BookingId { get; set; }
        public Guid TravellerId { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }
    }
}
