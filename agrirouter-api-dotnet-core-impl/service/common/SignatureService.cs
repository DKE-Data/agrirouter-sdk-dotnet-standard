using System;
using System.IO;
using System.Text;
using Agrirouter.Api.Exception;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;

namespace Agrirouter.Impl.Service.Common
{
    /// <summary>
    /// Service to create the signatures for the authorization process.
    /// </summary>
    public class SignatureService
    {
        /// <summary>
        /// Creating the agrirouter signature used for signing the requests.
        /// </summary>
        /// <param name="requestBody">The request body.</param>
        /// <param name="privateKey">The private key.</param>
        /// <returns>-</returns>
        public string XAgrirouterSignature(string requestBody, string privateKey)
        {
            return Hex.ToHexString(Signature(requestBody, privateKey));
        }

        /// <summary>
        /// Creating a common signature for the given request.
        /// </summary>
        /// <param name="requestBody">The request body.</param>
        /// <param name="privateKey">The private key.</param>
        /// <returns>-</returns>
        /// <exception cref="CouldNotCreateSignatureException">Will be thrown if the signature can not be created.</exception>
        public byte[] Signature(string requestBody, string privateKey)
        {
            try
            {
                var signer = SignerUtilities.GetSigner(Algorithm);
                signer.Init(true, GetPrivateKey(privateKey));
                signer.BlockUpdate(Encoding.UTF8.GetBytes(requestBody), 0, requestBody.Length);
                return signer.GenerateSignature();
            }
            catch (Exception e)
            {
                throw new CouldNotCreateSignatureException("Could not create signature.", e);
            }
        }

        /// <summary>
        /// Verify the created signature.
        /// </summary>
        /// <param name="requestBody">The request body.</param>
        /// <param name="signature">The formerly created signature.</param>
        /// <param name="publicKey">The public key.</param>
        /// <returns>-</returns>
        /// <exception cref="CouldNotVerifySignatureException">Will be thrown if the signature can not be created.</exception>
        public bool Verify(string requestBody, byte[] signature, string publicKey)
        {
            try
            {
                var signer = SignerUtilities.GetSigner(Algorithm);
                signer.Init(false, GetPublicKey(publicKey));
                signer.BlockUpdate(Encoding.UTF8.GetBytes(requestBody), 0, requestBody.Length);
                return signer.VerifySignature(signature);
            }
            catch (Exception e)
            {
                throw new CouldNotVerifySignatureException("Could not verify signature.", e);
            }
        }

        private static RsaPrivateCrtKeyParameters GetPrivateKey(string pem)
        {
            return (RsaPrivateCrtKeyParameters) new PemReader(new StringReader(pem)).ReadObject();
        }

        private static RsaKeyParameters GetPublicKey(string pem)
        {
            return (RsaKeyParameters) new PemReader(new StringReader(pem)).ReadObject();
        }

        private static string Algorithm => "SHA-256withRSA";
    }
}