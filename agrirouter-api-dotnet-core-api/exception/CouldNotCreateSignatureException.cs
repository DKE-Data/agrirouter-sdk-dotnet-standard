using System;

namespace com.dke.data.agrirouter.api.exception
{
    /**
     * Will be thrown if the signature can not be created.
     */
    [Serializable]
    public class CouldNotCreateSignatureException : Exception
    {
        public CouldNotCreateSignatureException()
        {
        }

        public CouldNotCreateSignatureException(string message) : base(message)
        {
        }

        public CouldNotCreateSignatureException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}