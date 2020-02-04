using System;
using System.IO;
using System.Reflection;
using System.Text;
using Agrirouter.Impl.service.Convinience;

namespace Agrirouter.Api.test.Data
{
    public class DataProvider
    {
        public static string ReadBase64EncodedImage()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                @"Data/Content/example.png");
            var allBytes = File.ReadAllBytes(path);
            return Encode.ToMessageContent(allBytes);
        }
    }
}