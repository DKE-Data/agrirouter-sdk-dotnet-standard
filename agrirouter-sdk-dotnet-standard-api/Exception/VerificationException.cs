using System;
using System.Net;
using Agrirouter.Api.Dto.Onboard;

namespace Agrirouter.Api.Exception
{
    /// <summary>
    ///     Will be thrown if the verification process is not successful.
    /// </summary>
    [Serializable]
    public class VerificationException : System.Exception
    {
        /// <summary> 
        ///     Constructor with error message only. 
        /// </summary> 
        /// <param name="message">-</param> 
        public VerificationException(string message) : base(message)
        {
        }
    }
}