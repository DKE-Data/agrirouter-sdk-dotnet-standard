using System.Collections.Generic;

namespace com.dke.data.agrirouter.api.service.parameters
{
    public class FeedConfirmParameters : MessageParameters
    {
        public List<string> MessageIds { get; set; }
    }
}