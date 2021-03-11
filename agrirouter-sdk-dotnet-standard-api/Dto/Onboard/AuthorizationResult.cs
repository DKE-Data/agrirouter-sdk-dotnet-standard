namespace Agrirouter.Api.Dto.Onboard
{
    /// <summary>
    ///     Data transfer object for the communication.
    /// </summary>
    public class AuthorizationResult
    {
        /// <summary>
        ///     State.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        ///     Signature.
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        ///     Token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        ///     Error.
        /// </summary>
        public string Error { get; set; }
    }
}