using System;
using Agrirouter.Api.Dto.Onboard.Inner;
using Newtonsoft.Json;

namespace Agrirouter.Api.Dto.Onboard
{
    /// <summary>
    /// A router device (you can create it in the AR).
    /// </summary>
    public class RouterDevice
    {
        /// <summary>
        /// The authentication properties.
        /// </summary>
        public Authentication Authentication { get; set; } = null!;

        /// <summary>
        /// The device alternate ID of the router device.
        /// </summary>
        public string DeviceAlternateId { get; set; } = null!;

        /// <summary>
        /// The connection criteria.
        /// </summary>
        public RouterDeviceConnectionCriteria ConnectionCriteria { get; set; } = null!;

        /// <summary>
        /// Parse a JSON string into a router device.
        /// </summary>
        /// <param name="json">The JSON from the AR.</param>
        /// <returns>The domain object.</returns>
        public static RouterDevice FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<RouterDevice>(json);
            }
            catch (JsonReaderException e)
            {
                throw new ArgumentException("Please check the JSON you provided, looks like it is not valid.", e);
            }
        }
    }
}