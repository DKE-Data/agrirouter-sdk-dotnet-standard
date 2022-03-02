using System.Threading;
using Agrirouter.Api.Service.Messaging;

namespace Agrirouter.Impl.Service.Messaging.CancellationToken
{
    /// <summary>
    /// A default implementation, based on tries and waiting time.
    /// </summary>
    public class DefaultCancellationToken : ICancellationToken
    {
        private readonly int _maxTries;
        private readonly int _waitTimeInMilliseconds;

        private int _nrOfRetries = 0;

        public DefaultCancellationToken(int maxTries, int waitTimeInMilliseconds)
        {
            _maxTries = maxTries;
            _waitTimeInMilliseconds = waitTimeInMilliseconds;
        }

        public bool IsNotCancelled()
        {
            return _nrOfRetries < _maxTries;
        }

        public void WaitIfNotCancelled()
        {
            if (IsNotCancelled())
            {
                Thread.Sleep(_waitTimeInMilliseconds);
            }
        }

        public void NextStep()
        {
            _nrOfRetries++;
        }
    }
}