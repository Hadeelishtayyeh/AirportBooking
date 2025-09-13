using AirportBookingSystem.Models;
using AirportBookingSystem.Repositories;
using AirportBookingSystem.Services;

Console.WriteLine("Welcome to the Airport Ticket Booking System");

Console.WriteLine("Are you a:");
Console.WriteLine("1. Passenger");
Console.WriteLine("2. Manager");

string? roleInput = Console.ReadLine();

switch (roleInput)
{
    case "1":
        await PassengerMenuAsync();
        break;
    case "2":
        await ManagerMenuAsync();
        break;
    default:
        Console.WriteLine("Invalid choice.");
        break;
}

async Task PassengerMenuAsync()
{
    Console.Write("Enter your name: ");
    string name = Console.ReadLine() ?? string.Empty;

    Console.Write("Enter your Passport Number: ");
    string passport = Console.ReadLine() ?? string.Empty;

    var passenger = new Passenger
    {
        Name = name,
        PassportNumber = passport
    };

    var flightRepo = new FlightRepository();
    var flightService = new FlightService(flightRepo);

    var flights = flightService.GetAllFlights();

    if (flights.Count == 0)
    {
        Console.WriteLine("No flights available.");
        return;
    }

    foreach (var f in flights)
    {
        Console.WriteLine($"ID: {f.FlightId}");
        Console.WriteLine($"From: {f.DepartureCountry} ({f.DepartureAirport})");
        Console.WriteLine($"To: {f.DestinationCountry} ({f.ArrivalAirport})");
        Console.WriteLine($"Departure Date: {f.DepartureDate.ToShortDateString()}");
        Console.WriteLine($"Economy: {f.PriceEconomy}, Business: {f.PriceBusiness}, First: {f.PriceFirstClass}");
        Console.WriteLine();
    }

    Console.Write("\nEnter Flight ID to book: ");
    string flightId = Console.ReadLine() ?? string.Empty;
    var selectedFlight = flights.Find(f => f.FlightId == flightId);

    if (selectedFlight == null)
    {
        Console.WriteLine("Flight not found.");
        return;
    }

    Console.Write("Choose class (Economy, Business, FirstClass): ");
    string classInput = Console.ReadLine();

    if (!Enum.TryParse(classInput, true, out BookingClass bookingClass))
    {
        Console.WriteLine("Invalid class.");
        return;
    }

    var booking = new Booking
    {
        Passenger = passenger,
        PassengerName = passenger.Name,
        PassportNumber = passenger.PassportNumber,
        Flight = selectedFlight,
        FlightId = selectedFlight.FlightId,
        BookingClass = bookingClass,
        BookingDate = DateTime.Now
    };

    Console.WriteLine("\nBooking Successful!");
    Console.WriteLine($"Booking ID: {booking.BookingId}");
    Console.WriteLine($"Passenger: {booking.PassengerName}");
    Console.WriteLine($"Flight: {selectedFlight.FlightId} | Class: {bookingClass}");
}

async Task ManagerMenuAsync()
{
    var flightRepo = new FlightRepository();
    var flightService = new FlightService(flightRepo);

    while (true)
    {
        Console.WriteLine("\nManager Menu:");
        Console.WriteLine("1. Import Flights ");
        Console.WriteLine("2. View All Flights");
        Console.WriteLine("3. Add Flight Manually");
        Console.WriteLine("4. Return to Main Menu");

        Console.Write("Enter your choice: ");
        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Enter CSV file path: ");
                string? path = Console.ReadLine();
                try
                {
                    await flightService.ImportFlightsFromCsvAsync(path ?? "");
                    Console.WriteLine("Flights imported successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error importing: {ex.Message}");
                }
                break;

            case "2":
                var flights = flightService.GetAllFlights();
                if (flights.Count == 0)
                {
                    Console.WriteLine("No flights available.");
                }
                else
                {
                    foreach (var f in flights)
                    {
                        Console.WriteLine($"ID: {f.FlightId}");
                        Console.WriteLine($"From: {f.DepartureCountry} ({f.DepartureAirport})");
                        Console.WriteLine($"To: {f.DestinationCountry} ({f.ArrivalAirport})");
                        Console.WriteLine($"Date: {f.DepartureDate.ToShortDateString()}");
                        Console.WriteLine($"Economy: {f.PriceEconomy}, Business: {f.PriceBusiness}, First: {f.PriceFirstClass}");
                        Console.WriteLine();
                    }
                }
                break;

            case "3":
                var newFlight = new Flight();

                Console.Write("Departure Country: ");
                newFlight.DepartureCountry = Console.ReadLine() ?? "";

                Console.Write("Destination Country: ");
                newFlight.DestinationCountry = Console.ReadLine() ?? "";

                Console.Write("Departure Airport: ");
                newFlight.DepartureAirport = Console.ReadLine() ?? "";

                Console.Write("Arrival Airport: ");
                newFlight.ArrivalAirport = Console.ReadLine() ?? "";

                Console.Write("Departure Date (yyyy-MM-dd): ");
                newFlight.DepartureDate = DateTime.Parse(Console.ReadLine() ?? "");

                Console.Write("Economy Price: ");
                newFlight.PriceEconomy = decimal.Parse(Console.ReadLine() ?? "0");

                Console.Write("Business Price: ");
                newFlight.PriceBusiness = decimal.Parse(Console.ReadLine() ?? "0");

                Console.Write("First Class Price: ");
                newFlight.PriceFirstClass = decimal.Parse(Console.ReadLine() ?? "0");

                flightService.AddFlight(newFlight);

                Console.WriteLine("Flight added successfully!");
                break;

            case "4":
                return;

            default:
                Console.WriteLine("Invalid choice. Try again.");
                break;
        }
    }
}
