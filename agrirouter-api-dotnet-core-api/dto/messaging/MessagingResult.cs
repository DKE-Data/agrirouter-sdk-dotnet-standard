using System.Collections.Generic;

namespace com.dke.data.agrirouter.api.dto.messaging
{
    /// <summary>
    /// Data transfer object for the communication.
    /// </summary>
    public class MessagingResult
    {
        public List<string> ApplicationMessageIds { get; set; }
    }
}