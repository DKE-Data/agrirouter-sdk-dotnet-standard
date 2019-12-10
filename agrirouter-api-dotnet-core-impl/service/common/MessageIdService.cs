using System;

namespace com.dke.data.agrirouter.impl.service.common
{
    public class MessageIdService
    {
        public string ApplicationMessageId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}