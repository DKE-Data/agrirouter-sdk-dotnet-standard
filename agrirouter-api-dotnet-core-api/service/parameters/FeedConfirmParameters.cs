using System.Collections.Generic;

namespace com.dke.data.agrirouter.api.service.parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public class FeedConfirmParameters : MessageParameters
    {
        public List<string> MessageIds { get; set; }
    }
}