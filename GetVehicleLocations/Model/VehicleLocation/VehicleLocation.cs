using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;

namespace GetVehicleLocations.Model
{
    public class VehicleLocation
    {
        [JsonPropertyName("Lines")]
        public string LINE_NR { get; set; }

        [JsonPropertyName("Brigade")]
        public string BRIGADE_NR { get; set; }

        [JsonPropertyName("VehicleNumber")]
        public string VEHICLE_NR { get; set; }

        [JsonPropertyName("Lon")]
        public decimal LONGITUDE { get; set; }

        [JsonPropertyName("Lat")]
        public decimal LATITUDE { get; set; }

        [JsonPropertyName("Time")]
        public DateTime GPS_STAMP { get; set; }

        public DateTime GENERATION_STAMP { get; set; }
        public int VEHICLE_TYPE { get; set; }

        public override string ToString()
        {
            return $"{LINE_NR},{BRIGADE_NR},{VEHICLE_NR},{LONGITUDE},{LATITUDE},{GPS_STAMP.ToString("yyyy-MM-ddTHH:mm:ss.fff")},{VEHICLE_TYPE}\n";
        }



    }


}
