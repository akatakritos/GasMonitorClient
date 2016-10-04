using System;
using System.Collections.Generic;
using System.Linq;

namespace GasTrackerDemo.Data
{
    public class VehicleWithStats
    {
        public VehicleStats Stats { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string VehicleType { get; set; }
    }
}