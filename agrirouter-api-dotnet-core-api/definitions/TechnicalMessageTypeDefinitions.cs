namespace com.dke.data.agrirouter.api.definitions
{
    /// <summary>
    /// Technical messages types for the messages.
    /// </summary>
    public static class TechnicalMessageTypes
    {
        /// <summary>
        /// Empty type for several messages.
        /// </summary>
        public static string Empty => "";

        /// <summary>
        /// Type 'dke:capabilities'.
        /// </summary>
        public static string DkeCapabilities => "dke:capabilities";
        
        /// <summary>
        /// Type 'dke:list_endpoints'.
        /// </summary>
        public static string DkeListEndpoints => "dke:list_endpoints";

        /// <summary>
        /// Type 'dke:list_endpoints_unfiltered'.
        /// </summary>
        public static string DkeListEndpointsUnfiltered => "dke:list_endpoints_unfiltered";

        /// <summary>
        /// Type 'dke:feed_header_query'.
        /// </summary>
        public static string DkeFeedHeaderQuery => "dke:feed_header_query";

        public static string DkeFeedMessageQuery => "dke:feed_message_query";

        public static string DkeFeedConfirm => "dke:feed_confirm";

        public static string DkeSubscription => "dke:subscription";

        public static string Iso11783TaskdataZip => "iso:11783:-10:taskdata:zip";

        public static string Iso11783DeviceDescriptionProtobuf => "iso:11783:-10:device_description:protobuf";

        public static string Iso11783TimeLogProtobuf => "iso:11783:-10:time_log:protobuf";

        public static string ImgBmp => "img:bmp";

        public static string ImgJpeg => "img:jpeg";

        public static string ImgPng => "img:png";

        public static string ShpShapeZip => "shp:shape:zip";

        public static string DocPdf => "doc:pdf";

        public static string VidAvi => "vid:avi";

        public static string VidMp4 => "vid:mp4";

        public static string VidWmv => "vid:wmv";
    }
}