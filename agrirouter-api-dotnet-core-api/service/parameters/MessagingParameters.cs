using System.Collections.Generic;

namespace com.dke.data.agrirouter.api.service.parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public class MessagingParameters : MessageParameters
    {
        public List<string> EncodedMessages { get; set; }
    }
}