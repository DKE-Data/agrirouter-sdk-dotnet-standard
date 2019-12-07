using Newtonsoft.Json.Serialization;

namespace com.dke.data.agrirouter.api.dto.messaging.inner
{
    public class Message
    {
        public string Content{ get; set; }
        
        public string Timestamp{ get; set; }
    }
}