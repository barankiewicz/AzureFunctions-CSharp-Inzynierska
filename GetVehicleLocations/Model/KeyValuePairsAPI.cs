using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace GetVehicleLocations.Model
{
    public class KeyValuePairsAPI
    {
        [JsonPropertyName("values")]
        public List<Dictionary<string, string>> values { get; set; }

        public VehicleStop GetVehicleStop()
        {
            var numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "." };
            return new VehicleStop()
            {
                LATITUDE = decimal.Parse(values[4].GetValueOrDefault("value"), numberFormatInfo),
                LONGITUDE = decimal.Parse(values[5].GetValueOrDefault("value"), numberFormatInfo),
                STOP_ID = values[0].GetValueOrDefault("value"),
                NAME = values[2].GetValueOrDefault("value"),
                STOP_NR = values[1].GetValueOrDefault("value"),
                STREET_ID = values[3].GetValueOrDefault("value"),
                DIRECTION = values[6].GetValueOrDefault("value"),
                VALID_FROM = DateTime.ParseExact(values[7].GetValueOrDefault("value"), "yyyy-MM-dd HH:mm:ss.f", null),
                GENERATION_STAMP = DateTime.Now
            };
        }

        public StopLine GetStopLine(VehicleStopId stop)
        {
            return new StopLine()
            {
                LINE_NR = values[0].GetValueOrDefault("value"),
                STOP_ID = stop.STOP_ID,
                STOP_NR = stop.STOP_NR,
                DATE = DateTime.Today
            };
        }
    }
}
