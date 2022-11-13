using GetVehicleLocations.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GetVehicleLocations.Converters
{
    public class VehicleStopConverter : JsonConverter<VehicleStop>
    {
        public override VehicleStop Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<Dictionary<string, string>> Data = new List<Dictionary<string, string>>();
            //reader.
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartArray:
                        break;
                    case JsonTokenType.StartObject:
                        break;

                }
            }


            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, VehicleStop value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
