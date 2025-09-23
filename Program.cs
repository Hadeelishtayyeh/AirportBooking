using AirportBookingSystem.Models;
using AirportBookingSystem.Repositories;
using AirportBookingSystem.Services;

Console.WriteLine("Welcome to the Airport Ticket Booking System");

Console.WriteLine("Choose role:");
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

// ... بداية الكود نفس الشيء

async Task ManagerMenuAsync()
{
    var flightRepo = new FlightRepository();
    var flightService = new FlightService(flightRepo);

    var bookingRepo = new BookingRepository();
    var bookingService = new BookingService(bookingRepo);

    var passengerRepo = new PassengerRepository();
    var passengerService = new PassengerService(passengerRepo);

    while (true)
    {
        Console.WriteLine("\nManager Menu:");
        Console.WriteLine("1. View All Flights");
        Console.WriteLine("2. Add Flight");
        Console.WriteLine("3. Update Flight");
        Console.WriteLine("4. Delete Flight");
        Console.WriteLine("5. View All Bookings");
        Console.WriteLine("6. Delete Booking");
        Console.WriteLine("7. View All Passengers");
        Console.WriteLine("8. Add Passenger");
        Console.WriteLine("9. Delete Passenger");
        Console.WriteLine("10. Exit");

        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                flightService.GetAllFlights().ForEach(f => Console.WriteLine($"{f.FlightId} | {f.DepartureCountry} -> {f.DestinationCountry}"));
                break;
            case "2":
                // إضافة رحلة جديدة (مثال)
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

            case "3":
                Console.Write("Enter Flight ID to update: ");
                var flightIdToUpdate = Console.ReadLine();

                var flights = flightService.GetAllFlights();
                var flightToUpdate = flights.Find(f => f.FlightId == flightIdToUpdate);

                if (flightToUpdate == null)
                {
                    Console.WriteLine("Flight not found.");
                    break;
                }

                var updatedFlight = new Flight();

                Console.Write("Departure Country: ");
                updatedFlight.DepartureCountry = Console.ReadLine() ?? flightToUpdate.DepartureCountry;

                Console.Write("Destination Country: ");
                updatedFlight.DestinationCountry = Console.ReadLine() ?? flightToUpdate.DestinationCountry;

                Console.Write("Departure Airport: ");
                updatedFlight.DepartureAirport = Console.ReadLine() ?? flightToUpdate.DepartureAirport;

                Console.Write("Arrival Airport: ");
                updatedFlight.ArrivalAirport = Console.ReadLine() ?? flightToUpdate.ArrivalAirport;

                Console.Write("Departure Date (yyyy-MM-dd): ");
                var depDateInput = Console.ReadLine();
                updatedFlight.DepartureDate = string.IsNullOrEmpty(depDateInput) ? flightToUpdate.DepartureDate : DateTime.Parse(depDateInput);

                Console.Write("Economy Price: ");
                var ecoPriceInput = Console.ReadLine();
                updatedFlight.PriceEconomy = string.IsNullOrEmpty(ecoPriceInput) ? flightToUpdate.PriceEconomy : decimal.Parse(ecoPriceInput);

                Console.Write("Business Price: ");
                var busPriceInput = Console.ReadLine();
                updatedFlight.PriceBusiness = string.IsNullOrEmpty(busPriceInput) ? flightToUpdate.PriceBusiness : decimal.Parse(busPriceInput);

                Console.Write("First Class Price: ");
                var firstPriceInput = Console.ReadLine();
                updatedFlight.PriceFirstClass = string.IsNullOrEmpty(firstPriceInput) ? flightToUpdate.PriceFirstClass : decimal.Parse(firstPriceInput);

                flightService.UpdateFlight(flightIdToUpdate, updatedFlight);
                Console.WriteLine("Flight updated successfully!");
                break;

            case "4":
                Console.Write("Enter Flight ID to delete: ");
                flightService.DeleteFlight(Console.ReadLine());
                Console.WriteLine("Flight deleted successfully!");
                break;

            case "5":
                bookingService.GetAll().ForEach(b => Console.WriteLine($"{b.BookingId}: {b.PassengerName} booked {b.FlightId}"));
                break;

            case "6":
                Console.Write("Enter Booking ID to delete: ");
                bookingService.DeleteBooking(Console.ReadLine());
                Console.WriteLine("Booking deleted successfully!");
                break;

            case "7":
                passengerService.GetAll().ForEach(p => Console.WriteLine($"{p.Name} - {p.PassportNumber}"));
                break;


            case "8":
                Console.Write("Enter Passport Number to delete: ");
                string? passDel = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(passDel))
                {
                    Console.WriteLine("Invalid passport number.");
                    break;
                }
                passengerService.Delete(passDel);
                Console.WriteLine("Passenger deleted if existed.");
                break;

            case "9":
                return;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
}

async Task PassengerMenuAsync()
{
    var flightService = new FlightService(new FlightRepository());
    var bookingService = new BookingService(new BookingRepository());
    var passengerService = new PassengerService(new PassengerRepository());

    while (true)
    {
        Console.WriteLine("\nPassenger Menu:");
        Console.WriteLine("1. View All Flights");
        Console.WriteLine("2. Book a Flight");
        Console.WriteLine("3. View My Bookings");
        Console.WriteLine("4. Cancel a Booking");
        Console.WriteLine("5. Exit");

        Console.Write("Enter choice: ");
        string choice = Console.ReadLine() ?? "";

        switch (choice)
        {
            case "1":
                var flights = flightService.GetAllFlights();
                if (flights.Count == 0) Console.WriteLine("No flights available.");
                else
                {
                    foreach (var f in flights)
                    {
                        Console.WriteLine($"ID: {f.FlightId} | {f.DepartureCountry} ({f.DepartureAirport}) -> {f.DestinationCountry} ({f.ArrivalAirport}) | Date: {f.DepartureDate:yyyy-MM-dd}");
                        Console.WriteLine($"Prices: Economy={f.PriceEconomy}, Business={f.PriceBusiness}, First={f.PriceFirstClass}");
                        Console.WriteLine(new string('-', 30));
                    }
                }
                break;

            case "2":
                Console.Write("Enter your full name: ");
                string passengerName = Console.ReadLine() ?? "";

                Console.Write("Enter your passport number: ");
                string passportNumber = Console.ReadLine() ?? "";

                // Add passenger if not exists
                var existingPassengers = passengerService.GetAll();
                if (!existingPassengers.Exists(p => p.PassportNumber == passportNumber))
                {
                    passengerService.AddPassenger(new Passenger { Name = passengerName, PassportNumber = passportNumber });
                }

                Console.Write("Enter Flight ID to book: ");
                string flightId = Console.ReadLine() ?? "";
                var flight = flightService.GetAllFlights().Find(f => f.FlightId == flightId);
                if (flight == null)
                {
                    Console.WriteLine("Flight not found.");
                    break;
                }

                Console.WriteLine("Choose class:");
                Console.WriteLine("1. Economy");
                Console.WriteLine("2. Business");
                Console.WriteLine("3. FirstClass");
                string classChoice = Console.ReadLine() ?? "";
                BookingClass bookingClass = classChoice switch
                {
                    "1" => BookingClass.Economy,
                    "2" => BookingClass.Business,
                    "3" => BookingClass.FirstClass,
                    _ => BookingClass.Economy
                };

                var newBooking = new Booking
                {
                    PassengerName = passengerName,
                    PassportNumber = passportNumber,
                    FlightId = flight.FlightId,
                    BookingClass = bookingClass,
                    BookingDate = DateTime.Now
                };

                bookingService.AddBooking(newBooking);
                Console.WriteLine("Booking completed.");
                break;

            case "3":
                Console.Write("Enter your passport number to view bookings: ");
                string passportSearch = Console.ReadLine() ?? "";

                var bookings = bookingService.GetAll();
                var myBookings = bookings.FindAll(b => b.PassportNumber == passportSearch);

                if (myBookings.Count == 0)
                {
                    Console.WriteLine("No bookings found for this passport number.");
                }
                else
                {
                    foreach (var b in myBookings)
                    {
                        Console.WriteLine($"BookingID: {b.BookingId} | FlightID: {b.FlightId} | Class: {b.BookingClass} | Date: {b.BookingDate}");
                    }
                }
                break;

            case "4":
                Console.Write("Enter Booking ID to cancel: ");
                string bookingToCancel = Console.ReadLine() ?? "";

                bookingService.DeleteBooking(bookingToCancel);
                Console.WriteLine("Booking cancelled if existed.");
                break;

            case "5":
                return;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
}