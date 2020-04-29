using System.Collections.Generic;
using Agrirouter.Api.Service.Parameters.Inner;

namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public class SendMultipleMessagesParameters : MessageParameters
    {
        /// <summary>
        /// Message entries.
        /// </summary>
        public List<MultipleMessageEntry> MultipleMessageEntries { get; set; }
    }
}