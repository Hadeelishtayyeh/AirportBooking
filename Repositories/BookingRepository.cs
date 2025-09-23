using System.Text.Json;
using AirportBookingSystem.Models;

namespace AirportBookingSystem.Repositories
{
    public class BookingRepository
    {
        private const string BookingsFile = "bookings.json";

        public List<Booking> LoadBookings()
        {
            if (!File.Exists(BookingsFile))
                return new List<Booking>();

            string json = File.ReadAllText(BookingsFile);
            return JsonSerializer.Deserialize<List<Booking>>(json) ?? new List<Booking>();
        }

        public void SaveBookings(List<Booking> bookings)
        {
            string json = JsonSerializer.Serialize(bookings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(BookingsFile, json);
        }
    }
}
