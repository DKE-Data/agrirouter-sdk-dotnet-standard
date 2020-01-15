using System.Collections.Generic;

namespace com.dke.data.agrirouter.api.service.parameters
{
    public class RevokeParameters
    {
        public string AccountId { get; set; }

        public List<string> EndpointIds { get; set; }

        public string ApplicationId { get; set; }
    }
}