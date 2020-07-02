using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Exception;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Agrirouter.Impl.Service.Common
{
    /// <summary>
    /// Service to create X509 certificates for the communication with the AR.
    /// </summary>
    public class X509CertificateService
    {
        /// <summary>
        /// Create a certificate for the given onboarding response from the AR.
        /// </summary>
        /// <param name="onboardResponse">-</param>
        /// <returns>A X509 certificate to use for the communication between endpoint and AR.</returns>
        /// <exception cref="CouldNotCreateCertificateForTypeException">-</exception>
        public static X509Certificate GetCertificate(OnboardResponse onboardResponse)
        {
            switch (onboardResponse.Authentication.Type)
            {
                case "P12":
                    return new X509Certificate2(
                        Convert.FromBase64String(onboardResponse.Authentication.Certificate),
                        onboardResponse.Authentication.Secret);
                case "PEM":
                {
                    var pemReader = new PemReader(
                        new StringReader(onboardResponse.Authentication.Certificate),
                        new PasswordFinder(onboardResponse.Authentication.Secret));
                    // Note the two subsequent read operations here...
                    // The first one will fail if the cast throws an exception, so
                    // there is no need to read a PemObject and check the type.
                    using var privateKey = DotNetUtilities.ToRSA(
                        (RsaPrivateCrtKeyParameters) pemReader.ReadObject());
                    var certificate = pemReader.ReadPemObject();
                    if (certificate.Type == "CERTIFICATE")
                    {
                        return new X509Certificate2(certificate.Content)
                            .CopyWithPrivateKey(privateKey);
                    }

                    break;
                }
            }

            throw new CouldNotCreateCertificateForTypeException(
                $"Could not create a certificate for the type '${onboardResponse.Authentication.Type}'");
        }

        /// <summary>
        /// Internal implementation of the password finder to pass the secret to the PEM reader.
        /// </summary>
        private class PasswordFinder : IPasswordFinder
        {
            private readonly string _password;

            public PasswordFinder(string password)
            {
                this._password = password;
            }

            public char[] GetPassword()
            {
                return _password.ToCharArray();
            }
        }
    }
}