using System.Collections.Generic;

namespace Agrirouter.Test.Data
{
    /// <summary>
    /// Identifier to save onboarding responses.
    /// </summary>
    public class Identifier
    {
        /// <summary>
        /// Get all identifier.
        /// </summary>
        public static List<string> AllCommunicationUnits
        {
            get
            {
                var all = new List<string>
                {
                    Http.CommunicationUnit.SingleEndpointWithoutRoute,
                    Http.CommunicationUnit.SingleEndpointWithPemCertificate,
                    Http.CommunicationUnit.SingleEndpointWithP12Certificate,
                    Http.CommunicationUnit.Recipient,
                    Http.CommunicationUnit.RecipientWithEnabledPushMessages,
                    Http.CommunicationUnit.Sender,
                    Http.CommunicationUnit.SenderWithMultipleRecipients
                };
                return all;
            }
        }
        
        /// <summary>
        /// Get all identifier.
        /// </summary>
        public static List<string> AllTelemetryPlatforms
        {
            get
            {
                var all = new List<string>
                {
                    Http.TelemtryPlatform.Sender
                };
                return all;
            }
        }

        /// <summary>
        /// HTTP based endpoints.
        /// </summary>
        public static class Http
        {
            /// <summary>
            /// Identifier for the onboarding responses used for integration tests regarding HTTP messaging with CUs.
            /// </summary>
            public static class CommunicationUnit
            {
                public static string SingleEndpointWithoutRoute => "Http/CommunicationUnit/SingleEndpointWithoutRoute";

                public static string SingleEndpointWithPemCertificate =>
                    "Http/CommunicationUnit/SingleEndpointWithPemCertificate";

                public static string SingleEndpointWithP12Certificate =>
                    "Http/CommunicationUnit/SingleEndpointWithP12Certificate";

                public static string Recipient => "Http/CommunicationUnit/Recipient";
                public static string RecipientWithEnabledPushMessages => "Http/CommunicationUnit/RecipientWithEnabledPushMessages";
                public static string Sender => "Http/CommunicationUnit/Sender";
                public static string SenderWithMultipleRecipients => "Http/CommunicationUnit/SenderWithMultipleRecipients";
            }

            public static class TelemtryPlatform
            {
                public static string Sender => "Http/TelemetryPlatform/Sender";
            }
        }
        
        /// <summary>
        /// Mqtt based endpoints.
        /// </summary>
        public static class Mqtt
        {
            /// <summary>
            /// Identifier for the onboarding responses used for integration tests regarding HTTP messaging with CUs.
            /// </summary>
            public static class CommunicationUnit
            {
                public static string SingleEndpointWithoutRoute => "Mqtt/CommunicationUnit/SingleEndpointWithoutRoute";
                public static string SingleEndpointWithPemCertificate =>
                    "Mqtt/CommunicationUnit/SingleEndpointWithPemCertificate";

                public static string SingleEndpointWithP12Certificate =>
                    "Mqtt/CommunicationUnit/SingleEndpointWithP12Certificate";

            }
        }
    }
}