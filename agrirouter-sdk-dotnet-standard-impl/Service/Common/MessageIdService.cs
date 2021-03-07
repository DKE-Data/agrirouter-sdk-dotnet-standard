using System;

namespace Agrirouter.Impl.Service.Common
{
    /// <summary>
    ///     Service to create messages IDs.
    /// </summary>
    public class MessageIdService
    {
        /// <summary>
        ///     Create an application message ID.
        /// </summary>
        /// <returns>-</returns>
        public static string ApplicationMessageId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}