using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using AirportBookingSystem.Models;

namespace AirportBookingSystem.Repositories
{
    public class FlightRepository
    {
        private const string FlightsFile = "flights.json";

        public List<Flight> LoadFlights()
        {
            if (!File.Exists(FlightsFile))
                return new List<Flight>();

            string json = File.ReadAllText(FlightsFile);
            var flights = JsonSerializer.Deserialize<List<Flight>>(json);
            return flights ?? new List<Flight>();
        }

        public void SaveFlights(List<Flight> flights)
        {
            string json = JsonSerializer.Serialize(flights, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FlightsFile, json);
        }
    }
}
