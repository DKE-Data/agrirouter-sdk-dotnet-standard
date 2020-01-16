namespace Agrirouter.Api.Dto.Onboard
{
    /// <summary>
    /// Data transfer object for the communication.
    /// </summary>
    public class AuthorizationResult
    {
        public string State { get; set; }
      
        public string Signature { get; set; }
      
        public string Token { get; set; }
     
        public string Error { get; set; }
    }
}