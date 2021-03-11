using System.Collections.Generic;

namespace Agrirouter.Api.Dto.Messaging
{
    /// <summary>
    ///     Data transfer object for the communication.
    /// </summary>
    public class MessagingResult
    {
        /// <summary>
        ///     Message IDs.
        /// </summary>
        public List<string> ApplicationMessageIds { get; set; }
    }
}