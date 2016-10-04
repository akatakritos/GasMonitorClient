using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace GasTrackerDemo.Data
{
    public class StatusResponse
    {
        public string Uptime { get; set; }
        public string Version { get; set; }

        // Use JsonProperty when you want to refer to a field
        // with a different name in C# than the API uses. In this
        // case, the API calls the field "commit" but we want to
        // use "GitCommit" inside our code

        [JsonProperty("commit")]
        public string GitCommit { get; set; }
    }
}