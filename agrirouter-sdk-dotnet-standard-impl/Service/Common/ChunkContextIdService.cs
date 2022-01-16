using System;

namespace Agrirouter.Impl.Service.Common
{
    /// <summary>
    ///     Service to create chunk context IDs.
    /// </summary>
    public class ChunkContextIdService
    {
        /// <summary>
        ///     Create an chunk context ID.
        /// </summary>
        /// <returns>-</returns>
        public static string ChunkContextId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}