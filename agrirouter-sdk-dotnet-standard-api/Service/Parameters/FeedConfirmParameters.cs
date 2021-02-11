using System.Collections.Generic;

namespace Agrirouter.Sdk.Api.Service.Parameters
{
    /// <summary>
    ///     Parameter container definition.
    /// </summary>
    public class FeedConfirmParameters : MessageParameters
    {
        /// <summary>
        ///     Message IDs.
        /// </summary>
        public List<string> MessageIds { get; set; }
    }
}