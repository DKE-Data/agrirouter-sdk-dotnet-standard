using System.Collections.Generic;

namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public class FeedConfirmParameters : MessageParameters
    {
        public List<string> MessageIds { get; set; }
    }
}