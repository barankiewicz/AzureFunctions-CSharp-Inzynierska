using System;
using System.Collections.Generic;
using System.Text;

namespace GetVehicleLocations.Model
{
    public class VehicleStop
    {
        public decimal LATITUDE { get; set; }
        public decimal LONGITUDE { get; set; }
        public string STOP_ID { get; set; }
        public string STOP_NR { get; set; }
        public string NAME { get; set; }
        public string STREET_ID { get; set; }
        public string DIRECTION { get; set; }
        public DateTime VALID_FROM { get; set; }
        public DateTime GENERATION_STAMP { get; set; }
    }
}
