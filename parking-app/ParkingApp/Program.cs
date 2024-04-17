using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        ParkingLot parkingLot = new ParkingLot(6);
        Console.WriteLine("Created a parking lot with 6 slots");
        Console.WriteLine("You can now interact with the parking system by entering commands in the console.");
        Console.WriteLine("Available commands: park, leave, status, reg_numbers_for_cars_with_colour, slot_numbers_for_cars_with_colour, slot_number_for_registration_number, slot_numbers_for_cars_with_colour");
        Console.WriteLine("Enter your commands:");

        while (true)
        {
            string input = Console.ReadLine();
            string[] tokens = input.Split(' ');

            if (tokens[0] == "park")
            {
                if (tokens.Length < 4)
                {
                    Console.WriteLine("Invalid command. Usage: park [registration_number] [color] [type]");
                    continue;
                }

                string registrationNumber = tokens[1];
                string color = tokens[2];
                string type = tokens[3];

                Vehicle vehicle = new Vehicle(registrationNumber, color, type);

                int slotNumber = parkingLot.ParkVehicle(vehicle);
                if (slotNumber != -1)
                {
                    Console.WriteLine($"Allocated slot number: {slotNumber}");
                }
                else
                {
                    Console.WriteLine("Sorry, parking lot is full");
                }
            }
            else if (tokens[0] == "leave")
            {
                if (tokens.Length < 2)
                {
                    Console.WriteLine("Invalid command. Usage: leave [slot_number]");
                    continue;
                }

                int slotNumber = int.Parse(tokens[1]);
                parkingLot.VacateSlot(slotNumber);
                Console.WriteLine($"Slot number {slotNumber} is free");
            }
            else if (tokens[0] == "status")
            {
                Console.WriteLine("Slot\tNo.\tType\tRegistration No\tColour");
                foreach (var slot in parkingLot.GetOccupiedSlots())
                {
                    Vehicle vehicle = slot.Vehicle;
                    Console.WriteLine($"{slot.Number}\t{vehicle.RegistrationNumber}\t{vehicle.Type}\t{vehicle.Color}");
                }
            }
            // Implement other commands (type_of_vehicles, registration_numbers_for_vehicles_with_ood_plate, etc.) similarly
            else if (tokens[0] == "exit")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid command");
            }
        }
    }
}

class ParkingLot
{
    private readonly List<ParkingSlot> slots;

    public ParkingLot(int capacity)
    {
        slots = new List<ParkingSlot>();
        for (int i = 0; i < capacity; i++)
        {
            slots.Add(new ParkingSlot(i + 1));
        }
    }

    public int ParkVehicle(Vehicle vehicle)
    {
        ParkingSlot slot = slots.FirstOrDefault(s => s.IsEmpty);
        if (slot != null)
        {
            slot.Park(vehicle);
            return slot.Number;
        }
        return -1; // Parking lot is full
    }

    public void VacateSlot(int slotNumber)
    {
        ParkingSlot slot = slots.FirstOrDefault(s => s.Number == slotNumber);
        if (slot != null)
        {
            slot.Vacate();
        }
    }

    public IEnumerable<ParkingSlot> GetOccupiedSlots()
    {
        return slots.Where(s => !s.IsEmpty);
    }
}

class ParkingSlot
{
    public int Number { get; }
    public Vehicle Vehicle { get; private set; }
    public bool IsEmpty => Vehicle == null;

    public ParkingSlot(int number)
    {
        Number = number;
    }

    public void Park(Vehicle vehicle)
    {
        Vehicle = vehicle;
    }

    public void Vacate()
    {
        Vehicle = null;
    }
}

class Vehicle
{
    public string RegistrationNumber { get; }
    public string Color { get; }
    public string Type { get; }

    public Vehicle(string registrationNumber, string color, string type)
    {
        RegistrationNumber = registrationNumber;
        Color = color;
        Type = type;
    }
}
