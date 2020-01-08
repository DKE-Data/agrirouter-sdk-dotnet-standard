using System;

namespace com.dke.data.agrirouter.impl.service.common
{
    /**
     * Service to create messages IDs.
     */
    public class MessageIdService
    {
        /**
         * Create an application message ID.
         */
        public static string ApplicationMessageId() => Guid.NewGuid().ToString();
    }
}