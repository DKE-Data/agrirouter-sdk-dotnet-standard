using System.Collections.Generic;
using com.dke.data.agrirouter.api.service.parameters.inner;

namespace com.dke.data.agrirouter.api.service.parameters
{
    public class SendMultipleMessagesParameters : MessageParameters
    {
        public List<MultipleMessageEntry> MultipleMessageEntries { get; set; }
    }
}