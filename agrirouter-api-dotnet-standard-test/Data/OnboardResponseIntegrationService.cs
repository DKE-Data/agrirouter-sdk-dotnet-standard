using System;
using System.IO;
using System.Reflection;
using System.Text;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Cloud.Registration;
using Agrirouter.Impl.Service.Convenience;
using Newtonsoft.Json;

namespace Agrirouter.Api.Test.Data
{
    /// <summary>
    ///     Service to read onboarding responses from a dedicated file system.
    /// </summary>
    public class OnboardResponseIntegrationService
    {
        /// <summary>
        /// Read an onboarding response using the given identifier
        /// </summary>
        /// <param name="identifier">Identifier.</param>
        /// <returns>Onboarding response if present, otherwise the service will throw an error.</returns>
        public static OnboardResponse Read(string identifier)
        {
            var path = PathToRead(identifier);
            var allBytes = File.ReadAllBytes(path);
            var json = Encoding.UTF8.GetString(allBytes);
            var onboardingResponse =
                JsonConvert.DeserializeObject(json, typeof(OnboardResponse));
            return onboardingResponse as OnboardResponse;
        }

        private static string PathToRead(string identifier)
        {
            var path = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                throw new InvalidOperationException(),
                @"Data/OnboardingResponses/" + identifier + ".json");
            return path;
        }

    }
}