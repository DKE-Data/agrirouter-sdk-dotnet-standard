using System;
using System.IO;
using Agrirouter.Commons;
using Agrirouter.Response;
using com.dke.data.agrirouter.api.dto.messaging;
using com.dke.data.agrirouter.api.exception;
using Google.Protobuf.WellKnownTypes;
using Serilog;

namespace com.dke.data.agrirouter.impl.service.common
{
    /**
     * Service for message decoding.
     */
    public class DecodeMessageService
    {
        /**
         * Decoding a Base64-encoded message which is containing a response envelope and a response payload wrapper. 
         */
        public DecodedMessage Decode(string rawMessage)
        {
            if (string.IsNullOrWhiteSpace(rawMessage))
            {
                throw new ArgumentException("Raw message data could not be null.");
            }

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

        /**
         * Parsing the inner message of the payload wrapper using the any object containing the content.
         */
        public Messages Decode(Any any)
        {
            try
            {
                return Messages.Parser.ParseFrom(any.Value);
            }
            catch (Exception e)
            {
                throw new CouldNotDecodeMessageException("There was an error during decoding of the ANY value.",e);
            }
        }
    }
}