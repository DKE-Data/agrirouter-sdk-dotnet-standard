namespace Agrirouter.Api.Service.Messaging
{
    /// <summary>
    /// An implementation for a cancellation token to run multiple attempts for fetching messages.
    /// </summary>
    public interface ICancellationToken
    {
        /// <summary>
        /// Signal for the polling process to cancel the polling.
        /// </summary>
        /// <returns>true if the polling can be cancelled, false otherwise.</returns>
        bool IsNotCancelled();

        /// <summary>
        /// Will wait a dedicated amount of time before starting the next step if the token is not cancelled.
        /// </summary>
        void WaitIfNotCancelled();

        /// <summary>
        /// Will be called after one step of the polling is completed and the next step is about to start.
        /// </summary>
        void NextStep();
    }
}