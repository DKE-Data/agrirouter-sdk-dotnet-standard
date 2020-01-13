using System.Collections.Generic;

namespace com.dke.data.agrirouter.api.dto.messaging
{
    /**
     * Result after sending the messages.
     */
    public class MessagingResult
    {
        public List<string> ApplicationMessageIds { get; set; }
    }
}