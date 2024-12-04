namespace ParkingSystem.Models
{
    public class ParkingSlot
    {
        public int SlotNumber { get; set; }
        public Vehicle? OccupiedVehicle { get; set; }

        public bool IsAvailable => OccupiedVehicle == null;
    }
}
