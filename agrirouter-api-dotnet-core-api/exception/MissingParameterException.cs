using System;

namespace com.dke.data.agrirouter.api.exception
{
    /**
     * Will be thrown if there are any missing parameters.
     */
    [Serializable]
    public class MissingParameterException : Exception
    {
        public MissingParameterException()
        {
        }

        public MissingParameterException(string message) : base(message)
        {
        }

        public MissingParameterException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}