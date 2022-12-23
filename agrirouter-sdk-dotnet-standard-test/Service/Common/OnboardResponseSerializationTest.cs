using Agrirouter.Api.Dto.Onboard;
using Newtonsoft.Json;
using Xunit;

namespace Agrirouter.Test.Service.Common;

public class OnboardResponseSerializationTest
{
    const string UppercaseOnboardResponseJson = @"{
  ""DeviceAlternateId"": ""deviceAlternateId"",
  ""CapabilityAlternateId"": ""capabilityAlternateId"",
  ""SensorAlternateId"": ""sensorAlternateId"",
  ""connectionCriteria"": {
    ""GatewayId"": ""gatewayId"",
    ""Measures"": ""measures"",
    ""Commands"": ""commands"",
    ""Host"": ""host"",
    ""Port"": ""port"",
    ""ClientId"": ""clientId""
  },
  ""authentication"": {
    ""Type"": ""type"",
    ""Secret"": ""secret"",
    ""Certificate"": ""certificate""
  }
}";
    const string LowercaseOnboardResponseJson = @"{
  ""deviceAlternateId"": ""deviceAlternateId"",
  ""capabilityAlternateId"": ""capabilityAlternateId"",
  ""sensorAlternateId"": ""sensorAlternateId"",
  ""connectionCriteria"": {
    ""gatewayId"": ""gatewayId"",
    ""measures"": ""measures"",
    ""commands"": ""commands"",
    ""host"": ""host"",
    ""port"": ""port"",
    ""clientId"": ""clientId""
  },
  ""authentication"": {
    ""type"": ""type"",
    ""secret"": ""secret"",
    ""certificate"": ""certificate""
  }
}";
    
    [Fact]
    void GivenAnOnboardResponseWithUppercasePropertyNamesThenDeserializationShouldWork()
    {
      var onboardResponse = JsonConvert.DeserializeObject<OnboardResponse>(UppercaseOnboardResponseJson);
      CheckAllFields(onboardResponse);
    }
    
    [Fact]
    void GivenAnOnboardResponseWithLowercasePropertyNamesThenDeserializationShouldWork()
    {
      var onboardResponse = JsonConvert.DeserializeObject<OnboardResponse>(LowercaseOnboardResponseJson);
      CheckAllFields(onboardResponse);
    }

    [Fact]
    void GivenAnOnboardResponseThenSerializationShouldBeLowercase()
    {
      var onboardResponse = JsonConvert.DeserializeObject<OnboardResponse>(UppercaseOnboardResponseJson);
      var onboardResponseSerialized = JsonConvert.SerializeObject(onboardResponse);
      Assert.Contains("\"deviceAlternateId\":", onboardResponseSerialized);
      Assert.Contains("\"capabilityAlternateId\":", onboardResponseSerialized);
      Assert.Contains("\"sensorAlternateId\":", onboardResponseSerialized);
      Assert.Contains("\"connectionCriteria\":", onboardResponseSerialized);
      Assert.Contains("\"gatewayId\":", onboardResponseSerialized);
      Assert.Contains("\"measures\":", onboardResponseSerialized);
      Assert.Contains("\"commands\":", onboardResponseSerialized);
      Assert.Contains("\"host\":", onboardResponseSerialized);
      Assert.Contains("\"port\":", onboardResponseSerialized);
      Assert.Contains("\"clientId\":", onboardResponseSerialized);
      Assert.Contains("\"authentication\":", onboardResponseSerialized);
      Assert.Contains("\"type\":", onboardResponseSerialized);
      Assert.Contains("\"secret\":", onboardResponseSerialized);
      Assert.Contains("\"certificate\":", onboardResponseSerialized);
      
      CheckAllFields(onboardResponse);
    }

    private void CheckAllFields(OnboardResponse onboardResponse)
    {
      Assert.Equal("deviceAlternateId", onboardResponse.DeviceAlternateId);
      Assert.Equal("capabilityAlternateId", onboardResponse.CapabilityAlternateId);
      Assert.Equal("sensorAlternateId", onboardResponse.SensorAlternateId);
      Assert.Equal("gatewayId", onboardResponse.ConnectionCriteria.GatewayId);
      Assert.Equal("measures", onboardResponse.ConnectionCriteria.Measures);
      Assert.Equal("commands", onboardResponse.ConnectionCriteria.Commands);
      Assert.Equal("host", onboardResponse.ConnectionCriteria.Host);
      Assert.Equal("port", onboardResponse.ConnectionCriteria.Port);
      Assert.Equal("clientId", onboardResponse.ConnectionCriteria.ClientId);
      Assert.Equal("type", onboardResponse.Authentication.Type);
      Assert.Equal("secret", onboardResponse.Authentication.Secret);
      Assert.Equal("certificate", onboardResponse.Authentication.Certificate);
    }
    
}