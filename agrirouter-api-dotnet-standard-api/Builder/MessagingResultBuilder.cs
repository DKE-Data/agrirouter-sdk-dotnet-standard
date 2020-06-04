using System.Collections.Generic;
using Agrirouter.Api.Dto.Messaging;

namespace Agrirouter.Api.Builder
{
    /// <summary>
    ///     Builder class for messaging results.
    /// </summary>
    public class MessagingResultBuilder
    {
        private readonly MessagingResult _messagingResult;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public MessagingResultBuilder()
        {
            _messagingResult = new MessagingResult {ApplicationMessageIds = new List<string>()};
        }

        /// <summary>
        ///     Add application message ID to the messaging result.
        /// </summary>
        /// <param name="applicationMessageId">-</param>
        /// <returns>-</returns>
        public MessagingResultBuilder WithApplicationMessageId(string applicationMessageId)
        {
            _messagingResult.ApplicationMessageIds.Add(applicationMessageId);
            return this;
        }

        /// <summary>
        ///     Build the result.
        /// </summary>
        /// <returns>-</returns>
        public MessagingResult Build()
        {
            return _messagingResult;
        }
    }
}