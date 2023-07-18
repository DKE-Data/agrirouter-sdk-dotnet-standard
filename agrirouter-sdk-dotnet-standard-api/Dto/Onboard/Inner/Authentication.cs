using Newtonsoft.Json;

namespace Agrirouter.Api.Dto.Onboard.Inner
{
    /// <summary>
    ///     Data transfer object for the communication.
    /// </summary>
    public class Authentication
    {
        /// <summary>
        ///     Type.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        ///     Secret.
        /// </summary>
        [JsonProperty(PropertyName = "secret")]
        public string Secret { get; set; }

        /// <summary>
        ///     Certificate.
        /// </summary>
        [JsonProperty(PropertyName = "certificate")]
        public string Certificate { get; set; }
    }
}