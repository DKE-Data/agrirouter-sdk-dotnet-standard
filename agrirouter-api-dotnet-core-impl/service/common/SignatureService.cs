using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using com.dke.data.agrirouter.api.exception;

namespace com.dke.data.agrirouter.impl.service.common
{
    public class SignatureService
    {
        public byte[] Create(string requestBody, string privateKey)
        {
            using (CngKey key = CngKey.Import(Encoding.UTF8.GetBytes(privateKey), CngKeyBlobFormat.Pkcs8PrivateBlob))
            using (RSA rsa = new RSACng(key))
            {
                return rsa.SignData(Encoding.UTF8.GetBytes(requestBody), HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1);
            }
        }
    }
}