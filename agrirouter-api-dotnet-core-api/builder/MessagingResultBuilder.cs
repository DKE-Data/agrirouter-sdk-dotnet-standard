using System.Collections.Generic;
using com.dke.data.agrirouter.api.dto.messaging;

namespace com.dke.data.agrirouter.api.builder
{
    /// <summary>
    /// Builder class for messaging results.
    /// </summary>
    public class MessagingResultBuilder
    {
        private readonly MessagingResult _messagingResult;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MessagingResultBuilder()
        {
            _messagingResult = new MessagingResult {ApplicationMessageIds = new List<string>()};
        }

        /// <summary>
        /// Add application message ID to the messaging result.
        /// </summary>
        /// <param name="applicationMessageId">-</param>
        /// <returns>-</returns>
        public MessagingResultBuilder WithApplicationMessageId(string applicationMessageId)
        {
            _messagingResult.ApplicationMessageIds.Add(applicationMessageId);
            return this;
        }

        /// <summary>
        /// Build the result.
        /// </summary>
        /// <returns>-</returns>
        public MessagingResult Build()
        {
            return _messagingResult;
        }
    }
}