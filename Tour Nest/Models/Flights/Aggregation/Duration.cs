namespace TourNest.Models.Flights.Aggregation
{
    public class Duration
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public string DurationType { get; set; }
        public bool Enabled { get; set; }
        public string ParamName { get; set; }

    }
}
