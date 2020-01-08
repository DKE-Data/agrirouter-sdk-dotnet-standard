using System;
using System.Runtime.Serialization;

namespace com.dke.data.agrirouter.api.exception
{
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