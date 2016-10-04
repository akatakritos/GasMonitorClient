﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GasTrackerDemo.Data
{
    public class FillUpResponse
    {
        public Guid Id { get; set; }
        public decimal Gallons { get; set; }
        public decimal Miles { get; set; }
        public bool PrimarilyHighway { get; set; }
        public DateTime FilledAt { get; set; }
    }
}