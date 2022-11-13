using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
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

namespace GetVehicleData
{
    public class GetRawTimetable
    {
        [FunctionName("GetRawTimetable")]
        public void Run([TimerTrigger("0 0 3 * * *")] TimerInfo myTimer, ILogger log)
        {
            var fileName = "RA" + DateTime.Today.AddDays(-1).ToString("yyMMdd") + ".7z";
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://rozklady.ztm.waw.pl/" + fileName);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential("anonymous", "******");
                request.KeepAlive = false;
                request.UseBinary = true;
                request.UsePassive = true;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();

                var sw = StorageManager.GetTimetableBlob(fileName);
                StreamReader reader = new StreamReader(responseStream);

                sw.Write(reader.ReadToEnd());

                sw.Close();
                reader.Close();
                response.Close(); 
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.Message.ToString());
            }
        }
    }
}
