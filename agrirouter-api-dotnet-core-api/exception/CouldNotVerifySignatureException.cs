using System;

namespace com.dke.data.agrirouter.api.exception
{
    /**
     * Will be thrown if the signature can not be verified.
     */
    [Serializable]
    public class CouldNotVerifySignatureException : Exception
    {
        public CouldNotVerifySignatureException()
        {
        }

        public CouldNotVerifySignatureException(string message) : base(message)
        {
        }

        public CouldNotVerifySignatureException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}