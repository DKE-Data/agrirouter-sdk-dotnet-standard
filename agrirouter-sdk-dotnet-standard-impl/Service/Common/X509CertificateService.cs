using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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

                    RSA privateKey;
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                        RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        privateKey = GivenToRsa((RsaPrivateCrtKeyParameters) pemReader.ReadObject());
                    }
                    else
                    {
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            privateKey = DotNetUtilities.ToRSA((RsaPrivateCrtKeyParameters) pemReader.ReadObject());
                        }
                        else
                        {
                            throw new CouldNotCreateCertificateForOsException(
                                $"Could not create a certificate for '${RuntimeInformation.OSDescription}'");
                        }
                    }

                    var certificate = pemReader.ReadPemObject();
                    if (certificate.Type == "CERTIFICATE")
                    {
                        return new X509Certificate2(certificate.Content).CopyWithPrivateKey(privateKey);
                    }

                    break;
                }
            }

            throw new CouldNotCreateCertificateForTypeException(
                $"Could not create a certificate for the type '${onboardResponse.Authentication.Type}'");
        }

        /// <summary>
        /// Internal implementation for Linux and OSX to support PEM certificate creation.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static RSA GivenToRsa(RsaPrivateCrtKeyParameters parameters)
        {
            var rp = DotNetUtilities.ToRSAParameters(parameters);
            return RSA.Create(rp);
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