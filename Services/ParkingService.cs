using ParkingSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace ParkingSystem.Services
{
    public class ParkingService
    {
        private readonly List<ParkingSlot> _slots;

        public ParkingService(int totalSlots)
        {
            _slots = Enumerable.Range(1, totalSlots)
                .Select(i => new ParkingSlot { SlotNumber = i })
                .ToList();
        }

        public string ParkVehicle(Vehicle vehicle)
        {
            var availableSlot = _slots.FirstOrDefault(s => s.IsAvailable);
            if (availableSlot == null)
                return "Sorry, parking lot is full";

            availableSlot.OccupiedVehicle = vehicle;
            return $"Allocated slot number: {availableSlot.SlotNumber}";
        }

        public string LeaveVehicle(int slotNumber)
        {
            var slot = _slots.FirstOrDefault(s => s.SlotNumber == slotNumber);
            if (slot == null || slot.IsAvailable)
                return "Slot not found or already free";

            slot.OccupiedVehicle = null;
            return $"Slot number {slotNumber} is free";
        }

        public List<ParkingSlot> GetStatus() => _slots;

        public IEnumerable<Vehicle> GetVehiclesByType(string type)
            => _slots.Where(s => !s.IsAvailable && s.OccupiedVehicle!.Type == type)
                     .Select(s => s.OccupiedVehicle!);

        public IEnumerable<string> GetRegistrationNumbersByColour(string colour)
            => _slots.Where(s => !s.IsAvailable && s.OccupiedVehicle!.Colour == colour)
                     .Select(s => s.OccupiedVehicle!.RegistrationNumber);

        public int? GetSlotNumberByRegistration(string registration)
            => _slots.FirstOrDefault(s => !s.IsAvailable &&
                                           s.OccupiedVehicle!.RegistrationNumber == registration)?.SlotNumber;

        public IEnumerable<string> GetRegistrationNumbersByOddEven(bool isOdd)
            => _slots.Where(s => !s.IsAvailable &&
                                 IsOdd(s.OccupiedVehicle!.RegistrationNumber, isOdd))
                     .Select(s => s.OccupiedVehicle!.RegistrationNumber);

        private bool IsOdd(string regNumber, bool isOdd)
        {
            var lastDigit = regNumber.LastOrDefault(c => char.IsDigit(c)) - '0';
            return isOdd ? lastDigit % 2 != 0 : lastDigit % 2 == 0;
        }
    }
}
