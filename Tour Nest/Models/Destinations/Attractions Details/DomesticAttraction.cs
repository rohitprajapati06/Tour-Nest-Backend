namespace TourNest.Models.Destinations.Attractions_Details
{

    public partial class DomesticAttraction
    {
        public int Id { get; set; }

        public string Destination { get; set; } = null!;

        public string Attraction1 { get; set; } = null!;

        public string Attraction2 { get; set; } = null!;

        public string? Attraction3 { get; set; }

        public string? Attraction4 { get; set; }

        public string? Attraction5 { get; set; }

        public string Heading1 { get; set; } = null!;

        public string Heading2 { get; set; } = null!;

        public string? Heading3 { get; set; }

        public string? Heading4 { get; set; }

        public string? Heading5 { get; set; }

        public string Paragraph1 { get; set; } = null!;

        public string Paragraph2 { get; set; } = null!;

        public string? Paragraph3 { get; set; }

        public string? Paragraph4 { get; set; }

        public string? Paragraph5 { get; set; }

        public string? AttractionHeading { get; set; }
    }

}
