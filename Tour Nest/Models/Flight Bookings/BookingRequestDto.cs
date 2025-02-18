namespace TourNest.Models.Flight_Bookings
{
    public class BookingRequestDto
    {
        public Guid UserId { get; set; }
        public Guid BookingId { get; set; }
        public DateTime TravellerDate { get; set; }
        public string DepartureAirport { get; set; } = null!;
        public string ArrivalAirport { get; set; } = null!;
        public string CabinClass { get; set; } = null!;
        public string PlaneNumber { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int Passenger { get; set; }
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
