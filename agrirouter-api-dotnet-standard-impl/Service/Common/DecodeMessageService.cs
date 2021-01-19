using System;
using System.IO;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Exception;
using Agrirouter.Commons;
using Agrirouter.Feed.Push.Notification;
using Agrirouter.Response;
using Google.Protobuf.WellKnownTypes;
using Serilog;

namespace Agrirouter.Impl.Service.Common
{
    /// <summary>
    ///     Service to decode messages and message contents.
    /// </summary>
    public class DecodeMessageService
    {
        /// <summary>
        ///     Decoding a Base64-encoded message which is containing a response envelope and a response payload wrapper.
        ///     <param name="rawMessage">The raw messages (Base64 encoded9).</param>
        ///     <returns>-</returns>
        ///     <exception cref="ArgumentException">Will be thrown if the input is not valid.</exception>
        ///     <exception cref="CouldNotDecodeMessageException">Will be thrown if the message can not be decoded.</exception>
        /// </summary>
        public static DecodedMessage Decode(string rawMessage)
        {
            if (string.IsNullOrWhiteSpace(rawMessage))
                throw new ArgumentException("Raw message data could not be null.");

            try
            {
                Log.Debug($"Start with the base64 decoding of the message '{rawMessage}'.");
                var rawProtoMessage = Convert.FromBase64String(rawMessage);
                Stream input = new MemoryStream(rawProtoMessage);

                Log.Debug($"Parse response envelope of the message '{rawMessage}'.");
                var responseEnvelope = ResponseEnvelope.Parser.ParseDelimitedFrom(input);

                Log.Debug($"Parse response payload wrapper of the message '{rawMessage}'.");
                var responsePayloadWrapper = ResponsePayloadWrapper.Parser.ParseDelimitedFrom(input);

                Log.Debug("Finish decoding of the message.");

                return new DecodedMessage
                {
                    ResponseEnvelope = responseEnvelope, ResponsePayloadWrapper = responsePayloadWrapper
                };
            }
            catch (Exception e)
            {
                throw new CouldNotDecodeMessageException("There was an error during decoding of the message.", e);
            }
        }

        /// <summary>
        ///     Parsing the push notification using the any object containing the content.
        /// </summary>
        /// <param name="any">The push message content from the decoded message.</param>
        /// <returns></returns>
        /// <exception cref="CouldNotDecodeMessageException">Will be thrown if the message content can not be decoded.</exception>
        public static PushNotification DecodePushNotification(Any any)
        {
            try
            {
                return PushNotification.Parser.ParseFrom(any.Value);
            }
            catch (Exception e)
            {
                throw new CouldNotDecodeMessageException("There was an error during decoding of the ANY value for the given push notification.", e);
            }
        }

        /// <summary>
        ///     Parsing the inner message of the payload wrapper using the any object containing the content.
        /// </summary>
        /// <param name="any">The message content from the decoded message.</param>
        /// <returns></returns>
        /// <exception cref="CouldNotDecodeMessageException">Will be thrown if the message content can not be decoded.</exception>
        public static Messages Decode(Any any)
        {
            try
            {
                return Messages.Parser.ParseFrom(any.Value);
            }
            catch (Exception e)
            {
                throw new CouldNotDecodeMessageException("There was an error during decoding of the ANY value.", e);
            }
        }
    }
}