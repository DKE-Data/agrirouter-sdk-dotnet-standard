using System;

namespace com.dke.data.agrirouter.impl.service.common
{
    /// <summary>
    /// Service to create messages IDs.
    /// </summary>
    public class MessageIdService
    {
        /// <summary>
        /// Create an application message ID.
        /// </summary>
        /// <returns>-</returns>
        public static string ApplicationMessageId() => Guid.NewGuid().ToString();
    }
}