using AirportBookingSystem.Models;
using AirportBookingSystem.Repositories;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace AirportBookingSystem.Services
{
    public class FlightService
    {
        private readonly FlightRepository _flightRepository;
        private static int _nextFlightId = 1;
        public FlightService(FlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
            var flights = _flightRepository.LoadFlights();
            if (flights.Count > 0)
            {
                _nextFlightId = flights.Max(f => f.FlightId) + 1;
            }
        }

        public List<Flight> GetAllFlights()
        {
            return _flightRepository.LoadFlights();
            
        }

        public void AddFlight(Flight flight)
        {
            var flights = _flightRepository.LoadFlights();
            flight.FlightId = _nextFlightId++;
            flights.Add(flight);
            _flightRepository.SaveFlights(flights);
        }

        public void UpdateFlight(int flightId, Flight updatedFlight)
        {
            var flights = _flightRepository.LoadFlights();
            var flight = flights.Find(f => f.FlightId == flightId);

            if (flight != null)
            {
                flight.DepartureCountry = updatedFlight.DepartureCountry;
                flight.DestinationCountry = updatedFlight.DestinationCountry;
                flight.DepartureDate = updatedFlight.DepartureDate;
                flight.DepartureAirport = updatedFlight.DepartureAirport;
                flight.ArrivalAirport = updatedFlight.ArrivalAirport;
                flight.PriceEconomy = updatedFlight.PriceEconomy;
                flight.PriceBusiness = updatedFlight.PriceBusiness;
                flight.PriceFirstClass = updatedFlight.PriceFirstClass;

                _flightRepository.SaveFlights(flights);
            }
        }

        public void DeleteFlight(int flightId)
        {

            var flights = _flightRepository.LoadFlights();
            flights.RemoveAll(f => f.FlightId == flightId);
            _flightRepository.SaveFlights(flights);
        }

        public async Task ImportFlightsFromCsvAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("CSV file not found.");

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            });

            var records = csv.GetRecords<FlightCsvRecord>().ToList();

            var flights = _flightRepository.LoadFlights();

            foreach (var record in records)
            {
                if (!DateTime.TryParse(record.DepartureDate, out DateTime departureDate))
                {
                    continue;
                }

                if (!decimal.TryParse(record.PriceEconomy, out decimal priceEconomy)) priceEconomy = 0;
                if (!decimal.TryParse(record.PriceBusiness, out decimal priceBusiness)) priceBusiness = 0;
                if (!decimal.TryParse(record.PriceFirstClass, out decimal priceFirstClass)) priceFirstClass = 0;

                var flight = new Flight
                {
                    FlightId = _nextFlightId++,
                    DepartureCountry = record.DepartureCountry,
                    DestinationCountry = record.DestinationCountry,
                    DepartureDate = departureDate,
                    DepartureAirport = record.DepartureAirport,
                    ArrivalAirport = record.ArrivalAirport,
                    PriceEconomy = priceEconomy,
                    PriceBusiness = priceBusiness,
                    PriceFirstClass = priceFirstClass
                };

                flights.Add(flight);
            }

            _flightRepository.SaveFlights(flights);
        }
    }
}
