using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
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
    public class GetVehicleLocationsBlob
    {
        [FunctionName("GetVehicleLocationsBlob")]
        public void Run([TimerTrigger("0,30 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            var buses = DataGetter.GetVehicleLocations(1);
            var trams = DataGetter.GetVehicleLocations(2);

            int rowNum = 0;

            var fileName = trams.result.Where(x => (DateTime.Now - x.GPS_STAMP).TotalMinutes < 1d).First().GPS_STAMP.ToString("yyyyMMdd") + ".csv";
            var blob = StorageManager.GetRawDataBlob(fileName);

            var sb = new StringBuilder();

            foreach (var bus in buses.result) //.Where(x => LineFilter.Lines.Contains(x.LINE_NR))
            {
                bus.VEHICLE_TYPE = 1;
                bus.GENERATION_STAMP = DateTime.Now;

                if ((DateTime.Now - bus.GPS_STAMP).TotalMinutes < 1d)
                {
                    sb.Append(bus.ToString());
                    rowNum++;
                }
            }

            blob.AppendBlock(StreamGenerator.GenerateStream(sb.ToString()));
            sb = new StringBuilder();

            foreach (var tram in trams.result) //.Where(x => LineFilter.Lines.Contains(x.LINE_NR))
            {
                tram.VEHICLE_TYPE = 2;
                tram.GENERATION_STAMP = DateTime.Now;

                if ((DateTime.Now - tram.GPS_STAMP).TotalMinutes < 1d)
                {
                    sb.Append(tram.ToString());
                    rowNum++;
                }
            }

            blob.AppendBlock(StreamGenerator.GenerateStream(sb.ToString()));
            log.LogInformation("Rows pushed: " + rowNum);
        }
    }

}
