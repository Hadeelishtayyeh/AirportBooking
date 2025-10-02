using AirportBookingSystem.Models;
using AirportBookingSystem.Repositories;
using System.Collections.Generic;

namespace AirportBookingSystem.Services
{
    public class PassengerService
    {
        private readonly PassengerRepository _passengerRepository;

        public PassengerService(PassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public List<Passenger> GetAll()
        {
            return _passengerRepository.LoadPassengers();
        }

        public void AddPassenger(Passenger passenger)
        {
            var passengers = _passengerRepository.LoadPassengers();
            passengers.Add(passenger);
            _passengerRepository.SavePassengers(passengers);
        }

        public void Delete(string? passportNumber)
        {
            if (string.IsNullOrEmpty(passportNumber)) return;

            var passengers = _passengerRepository.LoadPassengers();
            passengers.RemoveAll(p => p.PassportNumber == passportNumber);
            _passengerRepository.SavePassengers(passengers);
        }
    }
}
