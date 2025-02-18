namespace TourNest.Models.Flights.Luggage
{
    public class LuggageDetail
    {
        public string LuggageType { get; set; }
        public int MaxPiece { get; set; }
        public double MaxWeightPerPiece { get; set; }
        public string MassUnit { get; set; }
        public LuggageSizeRestrictions? SizeRestrictions { get; set; }
    }
}
