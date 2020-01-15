namespace com.dke.data.agrirouter.api.dto.onboard
{
    public class AuthorizationResult
    {
        public string State { get; set; }
      
        public string Signature { get; set; }
      
        public string Token { get; set; }
     
        public string Error { get; set; }
    }
}