using System.Collections.Generic;

namespace Agrirouter.Api.Service.Parameters
{
    public class OffboardVcuParameters : MessageParameters
    {
        public List<string> Endpoints { get; set; }
    }
}