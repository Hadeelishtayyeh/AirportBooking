namespace AirportBookingSystem.Models;

public class Booking
{
    public string? BookingId { get; set; }
    public string? PassengerName { get; set; }
    public string? PassportNumber { get; set; }  
    public Passenger? Passenger { get; set; }
    public Flight? Flight { get; set; }
    public string? FlightId { get; set; }
    public BookingClass BookingClass  { get; set; }
    public DateTime BookingDate { get; set; }
}
