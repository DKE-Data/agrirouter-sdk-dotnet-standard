using com.dke.data.agrirouter.api.dto.messaging;

namespace com.dke.data.agrirouter.api.service.messaging
{
    /**
     * Interface for all services sending messages.
     */
    public interface IMessagingService<in T>
    {
        /**
         * Sending a message using the given message parameters.
         */
        MessagingResult Send(T parameters);
    }
}