namespace AirportBookingSystem.Models
{
    public class Flight
    {
        public string? FlightId { get; set; }
        public string? DepartureCountry { get; set; }
        public string? DestinationCountry { get; set; }
        public DateTime DepartureDate { get; set; }
        public string? DepartureAirport { get; set; }
        public string? ArrivalAirport { get; set; }
        public decimal PriceEconomy { get; set; }
        public decimal PriceBusiness { get; set; }
        public decimal PriceFirstClass { get; set; }
    }
}
