namespace Agrirouter.Api.Exception
{
    /// <summary>
    ///     Will be thrown if the message to be sent is empty.
    /// </summary>
    public class CouldNotSendEmptyMessageException : System.Exception
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="message">-</param>
        public CouldNotSendEmptyMessageException(string message) : base(message)
        {
        }
    }
}