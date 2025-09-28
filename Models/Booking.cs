namespace AirportBookingSystem.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public string? PassengerName { get; set; }
        public string? PassportNumber { get; set; }
        public Passenger? Passenger { get; set; }
        public Flight? Flight { get; set; }
        public int FlightId { get; set; }
        public BookingClass BookingClass { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
