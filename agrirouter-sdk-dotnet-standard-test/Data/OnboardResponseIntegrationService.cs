using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Agrirouter.Api.Dto.Onboard;
using Newtonsoft.Json;
using Serilog;

namespace Agrirouter.Test.Data
{
    /// <summary>
    ///     Service to read onboard responses from a dedicated file system.
    /// </summary>
    public class OnboardResponseIntegrationService
    {
        /// <summary>
        /// Read an onboard response using the given identifier.
        /// </summary>
        /// <param name="identifier">Identifier.</param>
        /// <returns>Onboard response if present, otherwise the service will throw an error.</returns>
        public static OnboardResponse Read(string identifier)
        {
            var path = Path(identifier);
            var allBytes = File.ReadAllBytes(path);
            var json = Encoding.UTF8.GetString(allBytes);
            var onboardResponse =
                JsonConvert.DeserializeObject(json, typeof(OnboardResponse));
            return onboardResponse as OnboardResponse;
        }

        /// <summary>
        /// Save and update an onboard response using the given identifier.
        /// </summary>
        /// <param name="identifier">Identifier.</param>
        public static void Save(string identifier, OnboardResponse onboardResponse)
        {
            var path = Path(identifier);
            var json = JsonConvert.SerializeObject(onboardResponse);
            var fullFilePath = AppContext.BaseDirectory + @"../../../Data/OnboardingResponses/" + identifier + ".json";
            Console.WriteLine(fullFilePath);
            File.WriteAllText(
                fullFilePath,
                json);
        }

        /// <summary>
        /// Read all onboard responses.
        /// </summary>
        /// <returns>All onboard responses.</returns>
        public static List<OnboardResponse> AllCommunicationUnits()
        {
            var all = new List<OnboardResponse>();
            Identifier.AllCommunicationUnits.ForEach(identifier => { all.Add(Read(identifier)); });
            return all;
        }

        /// <summary>
        /// Read all onboard responses.
        /// </summary>
        /// <returns>All onboard responses.</returns>
        public static List<OnboardResponse> AllTelemetryPlatforms()
        {
            var all = new List<OnboardResponse>();
            Identifier.AllTelemetryPlatforms.ForEach(identifier => { all.Add(Read(identifier)); });
            return all;
        }

        private static string Path(string identifier)
        {
            var path = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                throw new InvalidOperationException(),
                @"Data/OnboardingResponses/" + identifier + ".json");
            return path;
        }
    }
}