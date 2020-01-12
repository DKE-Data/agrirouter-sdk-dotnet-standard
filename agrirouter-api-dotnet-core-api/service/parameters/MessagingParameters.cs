using System.Collections.Generic;

namespace com.dke.data.agrirouter.api.service.parameters
{
    /**
     * Parameter container definition.
     */
    public class MessagingParameters : MessageParameters
    {
        public List<string> EncodedMessages { get; set; }
    }
}