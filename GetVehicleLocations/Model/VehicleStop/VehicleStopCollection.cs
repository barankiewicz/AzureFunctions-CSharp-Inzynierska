using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GetVehicleLocations.Model
{
    public class VehicleStopCollection
    {
        [JsonPropertyName("result")]
        public KeyValuePairsAPI[] result { get; set; }
    }
}
