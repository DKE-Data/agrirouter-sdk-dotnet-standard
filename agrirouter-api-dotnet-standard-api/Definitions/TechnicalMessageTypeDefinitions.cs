namespace Agrirouter.Api.Definitions
{
    /// <summary>
    ///     Technical messages types for the messages.
    /// </summary>
    public static class TechnicalMessageTypes
    {
        /// <summary>
        ///     Empty type for several messages.
        /// </summary>
        public static string Empty => "";

        /// <summary>
        ///     Type 'dke:capabilities'.
        /// </summary>
        public static string DkeCapabilities => "dke:capabilities";

        /// <summary>
        ///     Type 'dke:list_endpoints'.
        /// </summary>
        public static string DkeListEndpoints => "dke:list_endpoints";

        /// <summary>
        ///     Type 'dke:list_endpoints_unfiltered'.
        /// </summary>
        public static string DkeListEndpointsUnfiltered => "dke:list_endpoints_unfiltered";

        /// <summary>
        ///     Type 'dke:feed_header_query'.
        /// </summary>
        public static string DkeFeedHeaderQuery => "dke:feed_header_query";

        /// <summary>
        ///     Type 'dke:feed_header_query'.
        /// </summary>
        public static string DkeFeedMessageQuery => "dke:feed_message_query";

        /// <summary>
        ///     Type 'dke:feed_confirm'.
        /// </summary>
        public static string DkeFeedConfirm => "dke:feed_confirm";

        /// <summary>
        ///     Type 'dke:subscription'.
        /// </summary>
        public static string DkeSubscription => "dke:subscription";

        /// <summary>
        ///     Type 'dke:cloud_onboard_endpoints'.
        /// </summary>
        public static string DkeCloudOnboardEndpoints => "dke:cloud_onboard_endpoints";

        /// <summary>
        ///     Type 'dke:cloud_offboard_endpoints'.
        /// </summary>
        public static string DkeCloudOffboardEndpoints => "dke:cloud_offboard_endpoints";

        /// <summary>
        ///     Type 'iso:11783:-10:taskdata:zip'.
        /// </summary>
        public static string Iso11783TaskdataZip => "iso:11783:-10:taskdata:zip";

        /// <summary>
        ///     Type 'iso:11783:-10:device_description:protobuf'.
        /// </summary>
        public static string Iso11783DeviceDescriptionProtobuf => "iso:11783:-10:device_description:protobuf";

        /// <summary>
        ///     Type 'iso:11783:-10:time_log:protobuf'.
        /// </summary>
        public static string Iso11783TimeLogProtobuf => "iso:11783:-10:time_log:protobuf";

        /// <summary>
        ///     Type 'img:bmp'.
        /// </summary>
        public static string ImgBmp => "img:bmp";

        /// <summary>
        ///     Type 'img:jpeg'.
        /// </summary>
        public static string ImgJpeg => "img:jpeg";

        /// <summary>
        ///     Type 'img:png'.
        /// </summary>
        public static string ImgPng => "img:png";

        /// <summary>
        ///     Type 'shp:shape:zip'.
        /// </summary>
        public static string ShpShapeZip => "shp:shape:zip";

        /// <summary>
        ///     Type 'doc:pdf'.
        /// </summary>
        public static string DocPdf => "doc:pdf";

        /// <summary>
        ///     Type 'vid:avi'.
        /// </summary>
        public static string VidAvi => "vid:avi";

        /// <summary>
        ///     Type 'vid:mp4'.
        /// </summary>
        public static string VidMp4 => "vid:mp4";

        /// <summary>
        ///     Type 'vid:wmv'.
        /// </summary>
        public static string VidWmv => "vid:wmv";

        /// <summary>
        ///     Type 'gps:info'.
        /// </summary>
        public static string GpsInfo => "gps:info";

        public static bool IsChunkable(string technicalMessageType)
        {
            return Iso11783TaskdataZip.Equals(technicalMessageType) || ImgBmp.Equals(technicalMessageType) ||
                   ImgJpeg.Equals(technicalMessageType) || ShpShapeZip.Equals(technicalMessageType) ||
                   DocPdf.Equals(technicalMessageType) || VidAvi.Equals(technicalMessageType) ||
                   VidMp4.Equals(technicalMessageType) || VidWmv.Equals(technicalMessageType);
        }
    }
}