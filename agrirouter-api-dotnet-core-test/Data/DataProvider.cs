using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Agrirouter.Api.test.Data
{
    public class DataProvider
    {
        public static string ReadBase64EncodedImage()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                @"Data/Content/example.png");
            var allBytes = File.ReadAllBytes(path);
            var imageAsBase64String = Convert.ToBase64String(allBytes);
            var base64EncodedMessageContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(imageAsBase64String));
            return base64EncodedMessageContent;
        }
    }
}