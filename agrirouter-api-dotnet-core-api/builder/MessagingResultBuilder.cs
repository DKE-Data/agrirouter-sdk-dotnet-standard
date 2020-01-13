using System.Collections.Generic;
using com.dke.data.agrirouter.api.dto.messaging;

namespace com.dke.data.agrirouter.api.builder
{
    public class MessagingResultBuilder
    {
        private readonly MessagingResult _messagingResult;

        public MessagingResultBuilder()
        {
            _messagingResult = new MessagingResult {ApplicationMessageIds = new List<string>()};
        }

        public MessagingResultBuilder WithApplicationMessageId(string applicationMessageId)
        {
            _messagingResult.ApplicationMessageIds.Add(applicationMessageId);
            return this;
        }

        public MessagingResult Build()
        {
            return _messagingResult;
        }
    }
}