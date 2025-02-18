namespace TourNest.Models.Flight_Bookings
{
    public class FlightBookingRequest
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int PassengerCount { get; set; }
    }
}
