using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GetVehicleData.Class
{
    public static class StreamGenerator
    {
        public static Stream GenerateStream(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
