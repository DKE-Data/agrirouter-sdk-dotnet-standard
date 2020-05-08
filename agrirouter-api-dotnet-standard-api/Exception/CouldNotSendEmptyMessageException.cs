namespace Agrirouter.Api.Exception
{
    /// <summary>
    /// Will be thrown if the message to be sent is empty.
    /// </summary>
    public class CouldNotSendEmptyMessageException: System.Exception
    {
        private string ErrorMessage { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="errorMessage">-</param>
        public CouldNotSendEmptyMessageException(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}