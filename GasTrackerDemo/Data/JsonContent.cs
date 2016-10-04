using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace GasTrackerDemo.Data
{
    public class JsonContent : StringContent
    {
        public JsonContent(string json) : base(json, Encoding.UTF8, "application/json")
        {
        }
    }
}