using System;
using MQTTnet.Client;

namespace Agrirouter.Api.Exception
{
    /// <summary>
    ///     Will be thrown if the message could not be sent to the AR.
    /// </summary>
    [Serializable]
    public class CouldNotSendMqttMessageException : System.Exception
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="reasonCode">-</param>
        /// <param name="message">-</param>
        public CouldNotSendMqttMessageException(MqttClientPublishReasonCode reasonCode, string message) : base(message)
        {
            ReasonCode = reasonCode;
        }

        public MqttClientPublishReasonCode ReasonCode { get; }
    }
}