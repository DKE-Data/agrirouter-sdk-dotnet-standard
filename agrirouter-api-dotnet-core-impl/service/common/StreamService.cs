using System.IO;

namespace com.dke.data.agrirouter.impl.service.common
{
    public class StreamService
    {
        public static Stream From(byte[] input)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(input);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}