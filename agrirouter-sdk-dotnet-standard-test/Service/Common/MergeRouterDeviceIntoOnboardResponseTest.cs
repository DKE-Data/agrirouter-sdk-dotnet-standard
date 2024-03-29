﻿using System;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Test.Data;
using Xunit;

namespace Agrirouter.Test.Service.Common
{
    /// <summary>
    /// Parse a router device.
    /// </summary>
    public class MergeRouterDeviceIntoOnboardResponseTest
    {
        private const string Json =
            "{\"authentication\":{\"type\":\"P12\",\"secret\":\"!oBTOQzcOXEYS?qZA5ioBu98QFMzvLQT!Zjr\",\"certificate\":\"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\"},\"deviceAlternateId\":\"dd5abe2d-06aa-40a3-9368-ab094305c539\",\"connectionCriteria\":{\"clientId\":\"7ea2a9f9-d6fd-40cd-8515-886a2c7864c8\",\"gatewayId\":\"2\",\"host\":\"dke-qa.eu10.cp.iot.sap\",\"port\":8883}}";

        [Fact]
        public void MergeValidRouterDeviceWithOnboardResponseShouldUpdateTheOnboardResponse()
        {
            var routerDevice = RouterDevice.FromJson(Json);
            var originalOnboardResponse =
                OnboardResponseIntegrationService.Read(Identifier.Mqtt.CommunicationUnit.SingleEndpointWithoutRoute);
            var mergedOnboardResponse = originalOnboardResponse.MergeWithRouterDevice(routerDevice);

            Assert.NotEqual(routerDevice.DeviceAlternateId, originalOnboardResponse.DeviceAlternateId);
            Assert.NotEqual(routerDevice.Authentication.Secret, originalOnboardResponse.Authentication.Secret);
            Assert.NotEqual(routerDevice.Authentication.Certificate,
                originalOnboardResponse.Authentication.Certificate);
            Assert.NotEqual(routerDevice.ConnectionCriteria.ClientId,
                originalOnboardResponse.ConnectionCriteria.ClientId);

            Assert.Equal(routerDevice.Authentication.Type, originalOnboardResponse.Authentication.Type);
            Assert.Equal(routerDevice.ConnectionCriteria.Host, originalOnboardResponse.ConnectionCriteria.Host);
            Assert.Equal(routerDevice.ConnectionCriteria.Port, originalOnboardResponse.ConnectionCriteria.Port);
            Assert.Equal(routerDevice.ConnectionCriteria.GatewayId,
                originalOnboardResponse.ConnectionCriteria.GatewayId);

            Assert.NotEqual(routerDevice.DeviceAlternateId, mergedOnboardResponse.DeviceAlternateId);

            Assert.Equal(routerDevice.Authentication.Type, mergedOnboardResponse.Authentication.Type);
            Assert.Equal(routerDevice.Authentication.Secret, mergedOnboardResponse.Authentication.Secret);
            Assert.Equal(routerDevice.Authentication.Certificate, mergedOnboardResponse.Authentication.Certificate);
            Assert.Equal(routerDevice.ConnectionCriteria.Host, mergedOnboardResponse.ConnectionCriteria.Host);
            Assert.Equal(routerDevice.ConnectionCriteria.Port, mergedOnboardResponse.ConnectionCriteria.Port);
            Assert.Equal(routerDevice.ConnectionCriteria.ClientId, mergedOnboardResponse.ConnectionCriteria.ClientId);
            Assert.Equal(routerDevice.ConnectionCriteria.GatewayId, mergedOnboardResponse.ConnectionCriteria.GatewayId);
        }

        [Fact]
        public void GivenInvalidGatewayIdWhenMergingRouterDeviceIntoOnboardResponseThenThereShouldBeAnError()
        {
            var routerDevice = RouterDevice.FromJson(Json);
            var originalOnboardResponse =
                OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.SingleEndpointWithoutRoute);

            Assert.Throws<ArgumentException>(() => originalOnboardResponse.MergeWithRouterDevice(routerDevice));
        }
    }
}