using System.Collections.Generic;

namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public class MessagingParameters : MessageParameters
    {
        public List<string> EncodedMessages { get; set; }
    }
}