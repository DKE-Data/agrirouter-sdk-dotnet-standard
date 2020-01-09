using Google.Protobuf.WellKnownTypes;

namespace com.dke.data.agrirouter.api.service.messaging
{
    /**
     * Decoding message responses for designated messages types.
     */
    public interface IDecodeMessageResponseService<out T>
    {
        /**
         * Decoding the given message using the given message parameters.
         */
        T Decode(Any messageResponse);
    }
}