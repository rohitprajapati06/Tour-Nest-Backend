using System.ComponentModel.DataAnnotations;

namespace TourNest.Models.Flights.Flight_Search_Request;
    public class FlightSearchRequest
    {
        [Required]
        public string FromId { get; set; }

        [Required]
        public string ToId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public string DepartDate { get; set; }

        [DataType(DataType.Date)]
        public string? ReturnDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be at least 1.")]
        public int? PageNo { get; set; } = 1;

        [Range(1, int.MaxValue, ErrorMessage = "Adults must be at least 1.")]
        public int? Adults { get; set; } = 1;

        public string? Children { get; set; }

        [EnumDataType(typeof(SortOptions), ErrorMessage = "Invalid sort option. Valid values are BEST, CHEAPEST, or FASTEST.")]
        public string? Sort { get; set; }

        [EnumDataType(typeof(CabinClassOptions), ErrorMessage = "Invalid cabin class. Valid values are ECONOMY, PREMIUM_ECONOMY, BUSINESS, or FIRST.")]
        public string? CabinClass { get; set; }

        [RegularExpression("INR", ErrorMessage = "Currency code must be 'INR'.")]
        public string? CurrencyCode { get; set; } = "INR";
    }

    public enum SortOptions
    {
        BEST,
        CHEAPEST,
        FASTEST
    }

    public enum CabinClassOptions
    {
        ECONOMY,
        PREMIUM_ECONOMY,
        BUSINESS,
        FIRST
    }

