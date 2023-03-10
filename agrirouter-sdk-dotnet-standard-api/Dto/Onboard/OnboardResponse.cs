using System;
using Agrirouter.Api.Dto.Onboard.Inner;
using Newtonsoft.Json;

namespace Agrirouter.Api.Dto.Onboard
{
    /// <summary>
    ///     Data transfer object for the communication.
    /// </summary>
    public class OnboardResponse
    {
        /// <summary>
        ///     Device alternate ID.
        /// </summary>
        [JsonProperty(PropertyName = "deviceAlternateId")]
        public string DeviceAlternateId { get; set; }

        /// <summary>
        ///     Capability alternate ID.
        /// </summary>
        [JsonProperty(PropertyName = "capabilityAlternateId")]
        public string CapabilityAlternateId { get; set; }

        /// <summary>
        ///     Sensor alternate ID.
        /// </summary>
        [JsonProperty(PropertyName = "sensorAlternateId")]
        public string SensorAlternateId { get; set; }

        /// <summary>
        ///     Connection criteria.
        /// </summary>
        [JsonProperty(PropertyName = "connectionCriteria")]
        public ConnectionCriteria ConnectionCriteria { get; set; }

        /// <summary>
        ///     Authentication.
        /// </summary>
        [JsonProperty(PropertyName = "authentication")]
        public Authentication Authentication { get; set; }

        /// <summary>
        /// Merge the data from the router device into this object.
        /// </summary>
        /// <param name="routerDevice">The router device</param>
        /// <returns>An updated onboard response.</returns>
        public OnboardResponse MergeWithRouterDevice(RouterDevice routerDevice)
        {
            if (string.Equals(ConnectionCriteria.GatewayId, routerDevice.ConnectionCriteria.GatewayId))
            {
                return new OnboardResponse()
                {
                    CapabilityAlternateId = CapabilityAlternateId,
                    DeviceAlternateId = DeviceAlternateId,
                    SensorAlternateId = SensorAlternateId,
                    ConnectionCriteria = new ConnectionCriteria()
                    {
                        Commands = ConnectionCriteria.Commands,
                        Measures = ConnectionCriteria.Measures,
                        Host = routerDevice.ConnectionCriteria.Host,
                        Port = routerDevice.ConnectionCriteria.Port,
                        ClientId = routerDevice.ConnectionCriteria.ClientId,
                        GatewayId = routerDevice.ConnectionCriteria.GatewayId
                    },
                    Authentication = routerDevice.Authentication
                };
            }
            else
            {
                throw new ArgumentException(
                    "The gateway ID of the router device does not match the gateway ID of the onboard response.");
            }
        }
    }
}