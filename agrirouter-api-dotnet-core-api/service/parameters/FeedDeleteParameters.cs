using System.Collections.Generic;
using Agrirouter.Feed.Request;

namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public class FeedDeleteParameters : MessageParameters
    {
        public List<string> Senders { get; set; }

        public List<string> MessageIds { get; set; }

        public ValidityPeriod ValidityPeriod { get; set; }
    }
}