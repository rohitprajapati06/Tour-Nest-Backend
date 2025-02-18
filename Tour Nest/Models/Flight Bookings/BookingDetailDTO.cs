using TourNest.Models.Payment_Details;

namespace TourNest.Models.Flight_Bookings
{
    public class BookingDetailDTO
    {
        public Guid UserId { get; set; }
        public Guid BookingId { get; set; }
        public DateTime TravellerDate { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public string CabinClass { get; set; }
        public string PlaneNumber { get; set; }
        public string Status { get; set; }
        public int Passenger { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<TravellerDetailDTO> TravellerDetails { get; set; }
        public List<PaymentDTO> Payments { get; set; }
    }
}
