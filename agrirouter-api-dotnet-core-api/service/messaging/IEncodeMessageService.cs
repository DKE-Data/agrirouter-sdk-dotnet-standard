using com.dke.data.agrirouter.api.dto.messaging;

namespace com.dke.data.agrirouter.api.service.messaging
{
    /**
     * Interface for all services encoding their own messages.
     */
    public interface IEncodeMessageService<in T>
    {
        /**
         * Encoding a message using the given message parameters.
         */
        EncodedMessage Encode(T parameters);
    }
}