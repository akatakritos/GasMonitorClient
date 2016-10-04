using System;
using System.Collections.Generic;
using System.Linq;

namespace GasTrackerDemo.Data
{
    public class OwnerResponse
    {
        public List<VehicleResponse> Vehicles { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}