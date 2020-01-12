using System.Collections.Generic;
using Agrirouter.Feed.Request;

namespace com.dke.data.agrirouter.api.service.parameters
{
    public class FeedDeleteParameters : SendMessageParameters
    {
        public List<string> Senders { get; set; }

        public List<string> MessageIds { get; set; }

        public ValidityPeriod ValidityPeriod { get; set; }
    }
}