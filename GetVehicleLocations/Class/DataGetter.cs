using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using GetVehicleLocations.Model;
using GetVehicleLocations.Converter;
using GetVehicleLocations.Converters;
using System.Linq;
using GetVehicleData.Class;

namespace GetVehicleLocations.Class
{
    public class DataGetter
    {
        private static string[] API_KEYS = new string[] {
            Environment.GetEnvironmentVariable("API_KEY_1"),
            Environment.GetEnvironmentVariable("API_KEY_2"),
            Environment.GetEnvironmentVariable("API_KEY_3"),
            Environment.GetEnvironmentVariable("API_KEY_4"),
            Environment.GetEnvironmentVariable("API_KEY_5")
        };
        private static string API_ENDPOINT_VEHICLES = "https://api.um.warszawa.pl/api/action/busestrams_get/";
        private static string RESOURCE_ID_VEHICLES = "f2e5503e-927d-4ad3-9500-4ab9e55deb59";
        private static readonly HttpClient client_vehicles = new HttpClient() { BaseAddress = new Uri(API_ENDPOINT_VEHICLES) };

        private static string API_ENDPOINT_STOPS = "https://api.um.warszawa.pl/api/action/dbstore_get/";
        private static string RESOURCE_ID_STOPS = "1c08a38c-ae09-46d2-8926-4f9d25cb0630";
        private static readonly HttpClient client_stops = new HttpClient() { BaseAddress = new Uri(API_ENDPOINT_STOPS) };

        private static string API_ENDPOINT_STOP_LINES = "https://api.um.warszawa.pl/api/action/dbtimetable_get/";
        private static string RESOURCE_ID_STOP_LINES = "88cd555f-6f31-43ca-9de4-66c479ad5942";
        private static readonly HttpClient client_stop_lines = new HttpClient() { BaseAddress = new Uri(API_ENDPOINT_STOP_LINES) };

        private static readonly Random r = new Random();

        public static VehicleLocationCollection GetVehicleLocations(int type) //1 for buses, 2 for trams
        {
            HttpResponseMessage response = client_vehicles.GetAsync($"?resource_id={RESOURCE_ID_VEHICLES}&apikey={API_KEYS[r.Next(API_KEYS.Length)]}&type={type}").Result;
            response.EnsureSuccessStatusCode();
            var options = new JsonSerializerOptions() { WriteIndented = true };
            options.Converters.Add(new CustomDateTimeConverter("yyyy-MM-dd HH:mm:ss"));
            VehicleLocationCollection result = JsonSerializer.Deserialize<VehicleLocationCollection>(response.Content.ReadAsStringAsync().Result, options);
            return result;
        }

        public static List<VehicleStop> GetStops()
        {
            var ret = new List<VehicleStop>();
            HttpResponseMessage response = client_stops.GetAsync($"?id={RESOURCE_ID_STOPS}&apikey={API_KEYS[r.Next(API_KEYS.Length)]}").Result;
            response.EnsureSuccessStatusCode();
            var options = new JsonSerializerOptions();
            options.Converters.Add(new CustomDateTimeConverter("yyyy-MM-dd HH:mm:ss"));

            VehicleStopCollection result = JsonSerializer.Deserialize<VehicleStopCollection>(response.Content.ReadAsStringAsync().Result, options);

            foreach(var stop in result.result)
                ret.Add(stop.GetVehicleStop());

            return ret;
        }

        public static List<StopLine> GetStopLines(VehicleStopId stop)
        {
            var ret = new List<StopLine>();
            HttpResponseMessage response = client_stop_lines.GetAsync($"?id={RESOURCE_ID_STOP_LINES}&busstopId={stop.STOP_ID}&busstopNr={stop.STOP_NR}&apikey={API_KEYS[r.Next(API_KEYS.Length)]}").Result;
            response.EnsureSuccessStatusCode();
            var res = response.Content.ReadAsStringAsync().Result;
            if (!res.StartsWith("    {\"result\":"))
                throw new Exception("Zapytanie API nie powiodlo sie");

            StopLineCollection result = JsonSerializer.Deserialize<StopLineCollection>(response.Content.ReadAsStringAsync().Result);

            foreach (var s in result.result)
                ret.Add(s.GetStopLine(stop));

            return ret.Where(x=>LineFilter.Lines.Contains(x.LINE_NR)).ToList();
        }
    }
}
