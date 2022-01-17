using System.Threading;
using Agrirouter.Api.Service.Messaging;

namespace Agrirouter.Impl.Service.Messaging.CancellationToken
{
    /// <summary>
    /// A default implementation, based on tries and waiting time.
    /// </summary>
    public class DefaultCancellationToken : ICancellationToken
    {
        private readonly int _maxRetries;
        private readonly int _waitTimeInMilliseconds;

        private int _nrOfRetries = 0;

        public DefaultCancellationToken(int maxRetries, int waitTimeInMilliseconds)
        {
            _maxRetries = maxRetries;
            _waitTimeInMilliseconds = waitTimeInMilliseconds;
        }

        public bool IsNotCancelled()
        {
            return _nrOfRetries < _maxRetries;
        }

        public void WaitBeforeNextStep()
        {
            Thread.Sleep(_waitTimeInMilliseconds);
        }

        public void NextStep()
        {
            _nrOfRetries++;
        }
    }
}