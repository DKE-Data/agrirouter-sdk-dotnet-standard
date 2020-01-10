using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using com.dke.data.agrirouter.api.exception;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;

namespace com.dke.data.agrirouter.impl.service.common
{
    public class SignatureService
    {
        public string XAgrirouterSignature(string requestBody, string privateKey)
        {
            return Hex.ToHexString(Signature(requestBody, privateKey));
        }
        
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