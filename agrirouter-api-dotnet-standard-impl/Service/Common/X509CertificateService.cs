using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Exception;
using Org.BouncyCastle.Crypto;
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
            if (onboardResponse.Authentication.Type.Equals("P12"))
            {
                return new X509Certificate2(
                    Convert.FromBase64String(onboardResponse.Authentication.Certificate),
                    onboardResponse.Authentication.Secret);
            }

            if (onboardResponse.Authentication.Type.Equals("PEM"))
            {
                var certificateWithoutPrivateKey = new X509Certificate2(
                    Encoding.UTF8.GetBytes(onboardResponse.Authentication.Certificate),
                    onboardResponse.Authentication.Secret);
                var rsa = ExtractRsaFromPem(onboardResponse.Authentication.Certificate,
                    onboardResponse.Authentication.Secret);
                return CombineCertificateAndRsaKey(certificateWithoutPrivateKey, rsa);
            }

            throw new CouldNotCreateCertificateForTypeException(
                $"Could not create a certificate for the type '${onboardResponse.Authentication.Type}'");
        }

        /// <summary>
        /// Extract RSA from PEM to enhance the certificate.
        /// </summary>
        /// <param name="pem">-</param>
        /// <param name="secret">-</param>
        /// <returns>RSA information extracted from the plain input.</returns>
        private static RSA ExtractRsaFromPem(string pem, string secret)
        {
            var rsa = RSA.Create();

            RSA MakePublicRcsp(RSA rcsp, RsaKeyParameters rkp)
            {
                var rsaParameters = DotNetUtilities.ToRSAParameters(rkp);
                rcsp.ImportParameters(rsaParameters);
                return rsa;
            }

            RSA MakePrivateRcsp(RSA rcsp, RsaPrivateCrtKeyParameters rkp)
            {
                var rsaParameters = DotNetUtilities.ToRSAParameters(rkp);
                rcsp.ImportParameters(rsaParameters);
                return rsa;
            }

            var pemReader = new PemReader(new StringReader(pem), new PasswordFinder(secret));
            var kp = pemReader.ReadObject();

            var hasPrivate = kp.GetType().GetProperty("Private") != null;
            var isPrivate = kp is RsaPrivateCrtKeyParameters;
            return isPrivate
                ? MakePrivateRcsp(rsa, (RsaPrivateCrtKeyParameters) kp)
                : hasPrivate
                    ? MakePrivateRcsp(rsa, (RsaPrivateCrtKeyParameters) (((AsymmetricCipherKeyPair) kp).Private))
                    : MakePublicRcsp(rsa, (RsaKeyParameters) kp);
        }

        private static X509Certificate2 CombineCertificateAndRsaKey(X509Certificate2 certificate, RSA rsa)
        {
            return certificate.CopyWithPrivateKey(rsa);
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