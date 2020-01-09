namespace com.dke.data.agrirouter.api.definitions
{
    /**
     * Technical messages types for the messages.
     */
    public static class TechnicalMessageTypes
    {
        public static string Empty => "";

        public static string DkeCapabilities => "dke:capabilities";

        public static string DkeListEndpoints => "dke:list_endpoints";

        public static string DkeFeedHeaderQuery => "dke:feed_header_query";

        public static string Iso11783TaskdataZip => "iso:11783:-10:taskdata:zip";
    }
}