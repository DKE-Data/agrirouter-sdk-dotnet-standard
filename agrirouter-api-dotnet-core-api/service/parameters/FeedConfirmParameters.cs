using System.Collections.Generic;
using Agrirouter.Feed.Request;
using Google.Protobuf.Collections;

namespace com.dke.data.agrirouter.api.service.parameters
{
    public class FeedConfirmParameters : SendMessageParameters
    {
        public List<string> MessageIds { get; set; }
    }
}