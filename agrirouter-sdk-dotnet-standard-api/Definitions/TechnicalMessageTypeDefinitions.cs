using System.Collections.Generic;

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
        ///     Type 'dke:feed_delete'.
        /// </summary>
        public static string DkeFeedDelete => "dke:feed_delete";

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

        /// <summary>
        /// Check whether the given technical message type is chunkable.
        /// </summary>
        /// <param name="technicalMessageType">Technical message type</param>
        /// <returns>True if chunkable, false otherwise.</returns>
        public static bool IsChunkable(string technicalMessageType)
        {
            return Iso11783TaskdataZip.Equals(technicalMessageType) || ImgBmp.Equals(technicalMessageType) ||
                   ImgJpeg.Equals(technicalMessageType) || ImgPng.Equals(technicalMessageType) ||
                   ShpShapeZip.Equals(technicalMessageType) || DocPdf.Equals(technicalMessageType) ||
                   VidAvi.Equals(technicalMessageType) || VidMp4.Equals(technicalMessageType) ||
                   VidWmv.Equals(technicalMessageType);
        }

        /// <summary>
        /// Returns all technical message types for which the endpoint can set capabilities.
        /// </summary>
        /// <returns>Technical message types.</returns>
        public static List<string> AllForCapabilitySetting()
        {
            return new List<string>
            {
                Iso11783TaskdataZip,
                ShpShapeZip,
                Iso11783DeviceDescriptionProtobuf,
                Iso11783TimeLogProtobuf,
                ImgBmp,
                ImgPng,
                ImgJpeg,
                DocPdf,
                VidAvi,
                VidMp4,
                VidWmv,
                GpsInfo
            };
        }

    }
}