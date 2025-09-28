namespace AirportBookingSystem.Services
{
    internal class FlightCsvRecord
    {
        public string? DepartureCountry { get; set; }
        public string? DestinationCountry { get; set; }
        public string? DepartureDate { get; set; }
        public string? DepartureAirport { get; set; }
        public string? ArrivalAirport { get; set; }
        public string? PriceEconomy { get; set; }
        public string? PriceBusiness { get; set; }
        public string? PriceFirstClass { get; set; }
    }
}
