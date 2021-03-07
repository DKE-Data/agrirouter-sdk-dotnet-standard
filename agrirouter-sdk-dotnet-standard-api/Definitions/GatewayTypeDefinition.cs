namespace Agrirouter.Api.Definitions
{
    /// <summary>
    ///     Several types of certificates used during the onboarding process.
    /// </summary>
    public static class GatewayTypeDefinition
    {
        /// <summary>
        ///     Type 'PEM'.
        /// </summary>
        public static string Http => "3";

        /// <summary>
        ///     Type 'P12'.
        /// </summary>
        public static string Mqtt => "2";
    }
}