using System.Collections.Generic;

namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public class OffboardVcuParameters : MessageParameters
    {
        /// <summary>
        /// Endpoints.
        /// </summary>
        public List<string> Endpoints { get; set; }
    }
}