namespace TourNest.Models.Flights.Search_Flights
{
    public class FlightLocatiion
    {
            public string Id { get; set; }
            public string Type { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
            public string City { get; set; }
            public string CityName { get; set; }
            public string Country { get; set; }
            public string CountryName { get; set; }
            public string CountryNameShort { get; set; }
            public string? PhotoUri { get; set; }
            public DistanceToCity? DistanceToCity { get; set; }
            public string? Parent { get; set; }
        
    }
}
