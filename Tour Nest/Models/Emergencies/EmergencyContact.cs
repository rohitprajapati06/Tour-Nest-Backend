namespace TourNest.Models.Emergencies
{
    public partial class EmergencyContact
    {
        public int Id { get; set; }

        public string Location { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string EmbassyPhone { get; set; } = null!;

        public string EmbassyEmail { get; set; } = null!;

        public string EmbassyAddress { get; set; } = null!;

        public long Police { get; set; }

        public long Fire { get; set; }

        public long Ambulance { get; set; }
    }
}
