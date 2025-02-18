using System;
using System.Linq;
using TourNest.Models.Flights.Search_Flights;
using TourNest.Models.Flights.Search_Flights.Segments;
using TourNest.Models.Flight_Bookings;
using System.Threading.Tasks;
using TourNest.Models;

namespace TourNest.Services.Flights.Book_Flights
{
    public class FlightBookingService
    {
        private readonly TourNestContext _context;

        public FlightBookingService(TourNestContext context)
        {
            _context = context;
        }

        public async Task<BookingRequestDto> MapFlightOfferToBookingRequestAsync(FlightOffers flightOffer, string email, string phone, Guid userId)
        {
            if (flightOffer == null || flightOffer.Segments == null || !flightOffer.Segments.Any())
                throw new ArgumentException("Flight offer is invalid or contains no segments.");

            var segment = flightOffer.Segments.First();
            var leg = segment.Legs?.FirstOrDefault();

            if (segment == null || leg == null)
                throw new InvalidOperationException("No valid segments or legs found in the flight offer.");

            int passengerCount = flightOffer.TravellerPrices?.Count ?? 0;

            var bookingDetail = new BookingDetail
            {
                BookingId = Guid.NewGuid(),
                UserId = userId,
                TravellerDate = segment.DepartureTime,
                DepartureAirport = segment.DepartureAirport?.Code,
                ArrivalAirport = segment.ArrivalAirport?.Code,
                CabinClass = leg.CabinClass,
                PlaneNumber = leg.FlightInfo.FlightNumber.ToString(),
                Status = "Confirmed",
                Passenger = passengerCount,
                Email = email,
                Phone = phone,
            };

            _context.BookingDetails.Add(bookingDetail);
            await _context.SaveChangesAsync();

            return new BookingRequestDto
            {
                UserId = bookingDetail.UserId,
                BookingId = bookingDetail.BookingId,
                TravellerDate = bookingDetail.TravellerDate,
                DepartureAirport = bookingDetail.DepartureAirport,
                ArrivalAirport = bookingDetail.ArrivalAirport,
                CabinClass = bookingDetail.CabinClass,
                PlaneNumber = bookingDetail.PlaneNumber,
                Status = bookingDetail.Status,
                Passenger = bookingDetail.Passenger,
                Email = bookingDetail.Email,
                Phone = bookingDetail.Phone
            };
        }
    }
}
