using System;

namespace com.dke.data.agrirouter.impl.service.common
{
    public class MessageIdService
    {
        public static string ApplicationMessageId() => Guid.NewGuid().ToString();
    }
}