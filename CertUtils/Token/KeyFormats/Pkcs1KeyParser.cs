using System;
using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace CertUtils.Token.KeyFormats
{
    public class Pkcs1KeyParser : IPrivateKeyParser
    {
        public bool CanParse(string privateKey)
        {
            if (string.IsNullOrWhiteSpace(privateKey))
                return false;

            return privateKey.TrimStart().StartsWith("-----BEGIN RSA PRIVATE KEY-----", 
                StringComparison.OrdinalIgnoreCase);
        }

        public RSAParameters Parse(string privateKey)
        {
            try
            {
                using var stringReader = new StringReader(privateKey);
                var pemReader = new PemReader(stringReader);
                var keyObject = pemReader.ReadObject();

                return keyObject switch
                {
                    AsymmetricCipherKeyPair keyPair when keyPair.Private is RsaPrivateCrtKeyParameters rsaPrivateCrtKeyParams =>
                        DotNetUtilities.ToRSAParameters(rsaPrivateCrtKeyParams),
                        
                    RsaPrivateCrtKeyParameters rsaPrivateKey =>
                        DotNetUtilities.ToRSAParameters(rsaPrivateKey),
                        
                    _ => throw new ArgumentException("Invalid PKCS#1 format: Unsupported key object type")
                };
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Failed to parse PKCS#1 private key", ex);
            }
        }
    }
}
