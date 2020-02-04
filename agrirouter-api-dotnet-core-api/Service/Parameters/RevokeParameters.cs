using System.Collections.Generic;

namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public class RevokeParameters
    {
        public string AccountId { get; set; }

        public List<string> EndpointIds { get; set; }

        public string ApplicationId { get; set; }
    }
}