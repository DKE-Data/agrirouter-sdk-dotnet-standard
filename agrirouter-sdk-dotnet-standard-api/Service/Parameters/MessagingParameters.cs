using System.Collections.Generic;

namespace Agrirouter.Sdk.Api.Service.Parameters
{
    /// <summary>
    ///     Parameter container definition.
    /// </summary>
    public class MessagingParameters : MessageParameters
    {
        /// <summary>
        ///     Encoded messages.
        /// </summary>
        public List<string> EncodedMessages { get; set; }
    }
}