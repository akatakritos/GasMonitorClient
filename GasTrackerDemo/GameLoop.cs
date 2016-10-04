using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using GasTrackerDemo.Data;

namespace GasTrackerDemo
{
    public class GameLoop
    {
        private readonly GasTrackerClient _client;
        private Guid _ownerId;

        public GameLoop(GasTrackerClient client)
        {
            _client = client;
        }

        public void Start()
        {
            _ownerId = GetOwnerId();
            var owner = _client.GetOwner(_ownerId);

            while (true)
            {
                var vehicles = _client.GetVehicles(_ownerId).ToArray();

                // 1. Edit Vehicle 1
                // 2. Edit Vehicle 2
                // ...
                // N. Edit Vehicle N
                // a. Add Vehicle
                // x. Exit

                Console.Clear();
                Console.WriteLine($"Hi, {owner.Name}. Welcome to your garage");
                int vehicleNumber = 1;
                foreach (var vehicle in vehicles)
                {
                    Console.WriteLine($"{vehicleNumber}. Edit {vehicle.Name} ({vehicle.Stats.AverageMilesPerGallon:F1} mpg)");
                    vehicleNumber++;
                }
                Console.WriteLine($"a. Add Vehicle");
                Console.WriteLine($"x. Exit");

                var choice = Console.ReadKey(intercept: true).KeyChar;

                if (choice == 'x') // exit
                    return;
                else if (choice == 'a') // add vehicle
                    AddVehicle();
                else if (choice > '0' && choice <= '9') // looks like they pressed a number
                {
                    var index = choice - '0' - 1;
                    if (index < vehicles.Length) // is it a valid number?
                        EditVehicleMenu(vehicles[choice - '0' - 1].Id);
                }
            }
        }

        private void AddVehicle()
        {
            Console.Clear();

            var vehicle = new CreateVehicleCommand();
            vehicle.Name = ReadString("Vehicle Name");
            vehicle.VehicleType = ReadString("Type (Truck, Car)");

            _client.CreateVehicle(_ownerId, vehicle);
        }

        private void EditVehicleMenu(Guid vehicleId)
        {
            while (true)
            {
                var vehicle = _client.GetVehicle(vehicleId);
                Console.Clear();

                Console.WriteLine(vehicle.Name);
                Console.WriteLine($"Fill Ups:      {vehicle.Stats.NumberOfFillups}");
                Console.WriteLine($"Total Miles:   {vehicle.Stats.TotalMiles}");
                Console.WriteLine($"Total Gallons: {vehicle.Stats.TotalsGallons}");
                Console.WriteLine($"Average MPG:   {vehicle.Stats.AverageMilesPerGallon}");
                Console.WriteLine();

                Console.WriteLine("f. Fill Up");
                Console.WriteLine("d. Delete");
                Console.WriteLine("x. Back to Main Menu");

                var choice = Console.ReadKey(intercept: true).KeyChar;
                switch (choice)
                {
                    case 'f':
                        FillUpMenu(vehicleId);
                        break;
                    case 'd':
                        _client.DeleteVehicle(vehicleId);
                        return;
                    case 'x':
                        return;
                }
            }
        }

        private void FillUpMenu(Guid vehicleId)
        {
            var record = new FillUpRecord();

            record.FilledAt = DateTime.UtcNow;
            record.Gallons = decimal.Parse(ReadString("Gallons"));
            record.Miles = decimal.Parse(ReadString("Miles"));
            record.PrimarilyHighway = true;

            _client.LogFillUp(vehicleId, record);
        }

        /// <summary>
        /// Get or create an owner
        /// </summary>
        /// <remarks>
        /// Checks a local file for the owner ID. If not found,
        /// prompt the user and create a new Owner through the API
        /// </remarks>
        /// <returns></returns>
        private Guid GetOwnerId()
        {
            if (File.Exists("ownerid.txt"))
            {
                // Found a saved owner, lets use that
                return Guid.Parse(File.ReadAllText("ownerid.txt"));
            }

            Console.WriteLine("Welcome! It looks like you haven't created an account yet.");
            Console.WriteLine("What's your name?");
            var name = Console.ReadLine();

            Console.WriteLine($"Welcome, {name}. Creating your account...");

            // Use the API client to create a new owner account
            var owner = _client.CreateOwner(new CreateOwnerCommand() { Name = name });

            // Save the owner account to the file system for next time
            File.WriteAllText("ownerid.txt", owner.Id.ToString("N"));

            Console.WriteLine("Done. Press any key to load the menu.");
            Console.ReadKey(intercept: true);

            return owner.Id;
        }

        private string ReadString(string message)
        {
            Console.Write($"{message}: ");
            return Console.ReadLine();
        }
    }
}