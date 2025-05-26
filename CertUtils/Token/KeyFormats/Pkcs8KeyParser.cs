using System;
using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace CertUtils.Token.KeyFormats
{
    public class Pkcs8KeyParser : IPrivateKeyParser
    {
        public bool CanParse(string privateKey)
        {
            if (string.IsNullOrWhiteSpace(privateKey))
                return false;

            var trimmedKey = privateKey.Trim();
            return trimmedKey.StartsWith("-----BEGIN PRIVATE KEY-----", 
                StringComparison.OrdinalIgnoreCase);
        }

        public RSAParameters Parse(string privateKey)
        {
            try
            {
                using var stringReader = new StringReader(privateKey);
                var pemReader = new PemReader(stringReader);
                
                // Read the key object
                var keyObj = pemReader.ReadObject();
                
                // Handle different key object types
                RsaPrivateCrtKeyParameters privateKeyParam = keyObj switch
                {
                    AsymmetricCipherKeyPair keyPair => keyPair.Private as RsaPrivateCrtKeyParameters,
                    RsaPrivateCrtKeyParameters rsaKey => rsaKey,
                    _ => null
                };

                if (privateKeyParam == null)
                {
                    throw new ArgumentException("Failed to parse PKCS#8 private key: Unsupported key format");
                }

                // Convert to RSAParameters
                return new RSAParameters
                {
                    Modulus = privateKeyParam.Modulus.ToByteArrayUnsigned(),
                    Exponent = privateKeyParam.PublicExponent?.ToByteArrayUnsigned() 
                        ?? throw new ArgumentException("Public exponent is missing in the private key"),
                    D = privateKeyParam.Exponent.ToByteArrayUnsigned(),
                    P = privateKeyParam.P?.ToByteArrayUnsigned() ?? throw new ArgumentException("Prime P is missing"),
                    Q = privateKeyParam.Q?.ToByteArrayUnsigned() ?? throw new ArgumentException("Prime Q is missing"),
                    DP = privateKeyParam.DP?.ToByteArrayUnsigned() ?? throw new ArgumentException("DP is missing"),
                    DQ = privateKeyParam.DQ?.ToByteArrayUnsigned() ?? throw new ArgumentException("DQ is missing"),
                    InverseQ = privateKeyParam.QInv?.ToByteArrayUnsigned() ?? throw new ArgumentException("QInv is missing")
                };
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to parse PKCS#8 private key: {ex.Message}", ex);
            }
        }
    }
}
