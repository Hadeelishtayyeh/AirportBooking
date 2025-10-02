using System.Text.Json;
using AirportBookingSystem.Models;

namespace AirportBookingSystem.Repositories
{
    public class PassengerRepository
    {
        private const string PassengersFile = "passengers.json";

        public List<Passenger> LoadPassengers()
        {
            if (!File.Exists(PassengersFile))
                return new List<Passenger>();

            string json = File.ReadAllText(PassengersFile);
            return JsonSerializer.Deserialize<List<Passenger>>(json) ?? new List<Passenger>();
        }

        public void SavePassengers(List<Passenger> passengers)
        {
            string json = JsonSerializer.Serialize(passengers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(PassengersFile, json);
        }
    }
}
