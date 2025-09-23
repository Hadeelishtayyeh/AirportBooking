using AirportBookingSystem.Models;
using AirportBookingSystem.Repositories;

namespace AirportBookingSystem.Services
{
    public class BookingService
    {
        private readonly BookingRepository _repository;

        public BookingService(BookingRepository repository)
        {
            _repository = repository;
        }

        public List<Booking> GetAll() => _repository.LoadBookings();

        public void AddBooking(Booking booking)
        {
            var bookings = _repository.LoadBookings();
            bookings.Add(booking);
            _repository.SaveBookings(bookings);
        }

        public void DeleteBooking(string bookingId)
        {
            var bookings = _repository.LoadBookings();
            var b = bookings.FirstOrDefault(x => x.BookingId == bookingId);
            if (b != null)
            {
                bookings.Remove(b);
                _repository.SaveBookings(bookings);
            }
        }
    }
}
