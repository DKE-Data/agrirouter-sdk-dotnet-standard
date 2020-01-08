using System;

namespace com.dke.data.agrirouter.api.exception
{
    /**
     * Will be thrown if a given message can not be decoded.
     */
    [Serializable]
    public class CouldNotDecodeMessageException : Exception
    {
        public CouldNotDecodeMessageException()
        {
        }

        public CouldNotDecodeMessageException(string message) : base(message)
        {
        }

        public CouldNotDecodeMessageException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}