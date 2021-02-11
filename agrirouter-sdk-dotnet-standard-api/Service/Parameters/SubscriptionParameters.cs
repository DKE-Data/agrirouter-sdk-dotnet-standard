using System.Collections.Generic;
using Agrirouter.Request.Payload.Endpoint;

namespace Agrirouter.Sdk.Api.Service.Parameters
{
    /// <summary>
    ///     Parameter container definition.
    /// </summary>
    public class SubscriptionParameters : MessageParameters
    {
        /// <summary>
        ///     Technical message types.
        /// </summary>
        public List<Subscription.Types.MessageTypeSubscriptionItem> TechnicalMessageTypes { get; set; }
    }
}