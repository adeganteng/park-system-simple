using System;
using ParkingSystem.Models;
using ParkingSystem.Services;

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to Parking System");
        ParkingService? parkingService = null;

        while (true)
        {
            var input = Console.ReadLine();
            if (input == null) continue;

            var parts = input.Split(' ');
            var command = parts[0];

            try
            {
                switch (command)
                {
                    case "create_parking_lot":
                        var totalSlots = int.Parse(parts[1]);
                        parkingService = new ParkingService(totalSlots);
                        Console.WriteLine($"Created a parking lot with {totalSlots} slots");
                        break;

                    case "park":
                        if (parkingService == null) throw new InvalidOperationException("No parking lot created.");
                        var vehicle = new Vehicle
                        {
                            RegistrationNumber = parts[1],
                            Colour = parts[2],
                            Type = parts[3]
                        };
                        Console.WriteLine(parkingService.ParkVehicle(vehicle));
                        break;

                    case "leave":
                        if (parkingService == null) throw new InvalidOperationException("No parking lot created.");
                        var slotNumber = int.Parse(parts[1]);
                        Console.WriteLine(parkingService.LeaveVehicle(slotNumber));
                        break;

                    case "status":
                        if (parkingService == null) throw new InvalidOperationException("No parking lot created.");
                        var status = parkingService.GetStatus();
                        Console.WriteLine("Slot\tNo.\tType\tRegistration No\tColour");
                        foreach (var slot in status.Where(s => !s.IsAvailable))
                        {
                            var v = slot.OccupiedVehicle!;
                            Console.WriteLine($"{slot.SlotNumber}\t{v.RegistrationNumber}\t{v.Type}\t{v.Colour}");
                        }
                        break;

                    case "exit":
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid command.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
