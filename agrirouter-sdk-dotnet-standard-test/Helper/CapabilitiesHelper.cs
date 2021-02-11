using System.Collections.Generic;
using Agrirouter.Request.Payload.Endpoint;
using Agrirouter.Sdk.Api.Definitions;
using Agrirouter.Sdk.Api.Service.Parameters.Inner;

namespace Agrirouter.Sdk.Test.Helper
{
    /// <summary>
    /// Helper to generate capabilities for capabilities messages.
    /// </summary>
    public class CapabilitiesHelper
    {
        /// <summary>
        /// Create capabilities parameters for all capabilities.
        /// </summary>
        public static List<CapabilityParameter> AllCapabilities
        {
            get
            {
                var all = new List<CapabilityParameter>();
                TechnicalMessageTypes.AllForCapabilitySetting().ForEach(technicalMessageType =>
                {
                    var capabilitiesParameter = new CapabilityParameter
                    {
                        Direction = CapabilitySpecification.Types.Direction.SendReceive,
                        TechnicalMessageType = technicalMessageType
                    };
                    all.Add(capabilitiesParameter);
                });
                return all;
            }
        }
    }
}