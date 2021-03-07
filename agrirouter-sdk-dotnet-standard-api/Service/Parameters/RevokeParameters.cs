using System.Collections.Generic;

namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    ///     Parameter container definition.
    /// </summary>
    public class RevokeParameters
    {
        /// <summary>
        ///     Account ID.
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        ///     Endpoint IDs.
        /// </summary>
        public List<string> EndpointIds { get; set; }

        /// <summary>
        ///     Application ID.
        /// </summary>
        public string ApplicationId { get; set; }
    }
}