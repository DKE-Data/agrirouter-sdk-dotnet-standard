using System;
using MQTTnet.Client.Publishing;

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
        /// <param name="errorMessage">-</param>
        public CouldNotSendMqttMessageException(MqttClientPublishReasonCode reasonCode, string errorMessage)
        {
            ReasonCode = reasonCode;
            ErrorMessage = errorMessage;
        }

        private MqttClientPublishReasonCode ReasonCode { get; }

        private string ErrorMessage { get; }
    }
}