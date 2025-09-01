using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AirportBookingSystem.Models
{
    public class Manager
    {
        private const string FlightsFile = "flights.json";

        public List<Flight> LoadFlights()
        {
            if (!File.Exists(FlightsFile))
                return new List<Flight>();

            string json = File.ReadAllText(FlightsFile);
            return JsonSerializer.Deserialize<List<Flight>>(json);
        }

        public void SaveFlights(List<Flight> flights)
        {
            string json = JsonSerializer.Serialize(flights, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FlightsFile, json);
        }

        public void AddFlight(Flight flight)
        {
            var flights = LoadFlights();
            flights.Add(flight);
            SaveFlights(flights);
        }

        public void ImportFlightsFromCSV(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found.");
                return;
            }

            var lines = File.ReadAllLines(filePath);
            var flights = LoadFlights();
            int importedCount = 0;

            for (int i = 1; i < lines.Length; i++) // Skip header
            {
                var line = lines[i];
                var parts = line.Split(',');

                if (parts.Length != 8)
                {
                    Console.WriteLine($"Line {i + 1} skipped: Invalid format.");
                    continue;
                }

                try
                {
                    Flight flight = new Flight
                    {
                        FlightId = Guid.NewGuid().ToString(),
                        DepartureCountry = parts[0],
                        DestinationCountry = parts[1],
                        DepartureDate = DateTime.Parse(parts[2]),
                        DepartureAirport = parts[3],
                        ArrivalAirport = parts[4],
                        PriceEconomy = decimal.Parse(parts[5]),
                        PriceBusiness = decimal.Parse(parts[6]),
                        PriceFirstClass = decimal.Parse(parts[7])
                    };
                    flights.Add(flight);
                    importedCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error on line {i + 1}: {ex.Message}");
                }
            }

            SaveFlights(flights);
            Console.WriteLine($"Imported {importedCount} flights successfully.");
        }
    }
}
