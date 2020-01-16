using System.Collections.Generic;
using Agrirouter.Request.Payload.Endpoint;

namespace com.dke.data.agrirouter.api.service.parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public class SubscriptionParameters : MessageParameters
    {
        public List<Subscription.Types.MessageTypeSubscriptionItem> TechnicalMessageTypes { get; set; }
    }
}