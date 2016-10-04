using System;
using System.Collections.Generic;
using System.Linq;

namespace GasTrackerDemo.Data
{
    public class VehicleStats
    {
        public decimal TotalMiles { get; set; }
        public decimal TotalsGallons { get; set; }
        public int NumberOfFillups { get; set; }
        public decimal AverageMilesPerGallon { get; set; }
    }
}