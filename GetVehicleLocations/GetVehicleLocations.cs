using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GetVehicleData.Class;
using GetVehicleLocations.Class;
using GetVehicleLocations.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace GetVehicleLocations
{
    public class GetVehicleLocations
    {
        [FunctionName("GetVehicleLocations")]
        public void Run([TimerTrigger("0,30 * * * * *")]TimerInfo myTimer, ILogger log, [Sql("dbo.VEHICLE_LOCATIONS", ConnectionStringSetting = "SqlConnectionString")] ICollector<VehicleLocation> LocationsTable)
        {
            var buses = DataGetter.GetVehicleLocations(1);
            var trams = DataGetter.GetVehicleLocations(2);

            int rowNum = 0;

            foreach (var bus in buses.result) //.Where(x => LineFilter.Lines.Contains(x.LINE_NR))
            {
                bus.VEHICLE_TYPE = 1;
                bus.GENERATION_STAMP = DateTime.Now;

                if ((DateTime.Now - bus.GPS_STAMP).TotalMinutes < 1d)
                {
                    LocationsTable.Add(bus);
                    rowNum++;
                }
            }

            foreach (var tram in trams.result) //.Where(x => LineFilter.Lines.Contains(x.LINE_NR))
            {
                tram.VEHICLE_TYPE = 2;
                tram.GENERATION_STAMP = DateTime.Now;

                if ((DateTime.Now - tram.GPS_STAMP).TotalMinutes < 1d)
                {
                    LocationsTable.Add(tram);
                    rowNum++;
                }
            }

            log.LogInformation("Rows pushed: " + rowNum);
        }
    }

}
