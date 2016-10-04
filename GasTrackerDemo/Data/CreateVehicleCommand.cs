using System;
using System.Collections.Generic;
using System.Linq;

namespace GasTrackerDemo.Data
{
    public class CreateVehicleCommand
    {
        public string Name { get; set; }
        public string VehicleType { get; set; }
    }
}