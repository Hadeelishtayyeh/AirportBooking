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

        public FlightService(FlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public List<Flight> GetAllFlights()
        {
            return _flightRepository.LoadFlights();
        }

        public void AddFlight(Flight flight)
        {
            var flights = _flightRepository.LoadFlights();
            flight.FlightId = Guid.NewGuid().ToString();
            flights.Add(flight);
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
                var flight = new Flight
                {
                    FlightId = Guid.NewGuid().ToString(),
                    DepartureCountry = record.DepartureCountry,
                    DestinationCountry = record.DestinationCountry,
                    DepartureDate = DateTime.Parse(record.DepartureDate),
                    DepartureAirport = record.DepartureAirport,
                    ArrivalAirport = record.ArrivalAirport,
                    PriceEconomy = decimal.Parse(record.PriceEconomy),
                    PriceBusiness = decimal.Parse(record.PriceBusiness),
                    PriceFirstClass = decimal.Parse(record.PriceFirstClass)
                };

                flights.Add(flight);
            }

            _flightRepository.SaveFlights(flights);
        }
    }
}
