namespace com.dke.data.agrirouter.api.service
{
    /**
     * Interface for all services sending messages.
     */
    public interface IMessagingService<T>
    {
        string send(T parameters);
    }
}