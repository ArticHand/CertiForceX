using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace CertUtils.Token
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly string _clientId;
        private readonly string _username;
        private readonly string _authEndpoint;
        private readonly string _aud;

        public JwtTokenGenerator(string clientId, string username, string aud, string authEndpoint)
        {
            _clientId = clientId;
            _username = username;
            _aud = aud;
            _authEndpoint = authEndpoint;
        }

        public string GenerateJwtToken(string privateKey)
        {
            var now = DateTime.UtcNow;

            // Use RSA.Create() for platform-independent key generation
            using var rsa = RSA.Create();
            rsa.ImportParameters(GetRsaParametersFromPrivateKey(privateKey)); // Import RSA parameters from private key

            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            var jwtHeader = new JwtHeader(signingCredentials);
            var jwtPayload = new JwtPayload
        {
            { "iss", _clientId },
            { "sub", _username },
            { "aud", _aud },
            { "exp", new DateTimeOffset(now.AddMinutes(3)).ToUnixTimeSeconds() },
            { "iat", new DateTimeOffset(now).ToUnixTimeSeconds() }
        };

            var jwtToken = new JwtSecurityToken(jwtHeader, jwtPayload);
            var jwtHandler = new JwtSecurityTokenHandler();
            return jwtHandler.WriteToken(jwtToken);
        }

        private RSAParameters GetRsaParametersFromPrivateKey(string privateKey)
        {
            var pemReader = new PemReader(new StringReader(privateKey));
            var keyObject = pemReader.ReadObject();

            if (keyObject is AsymmetricCipherKeyPair keyPair)
            {
                var rsaPrivateCrtKeyParams = (RsaPrivateCrtKeyParameters)keyPair.Private;
                return DotNetUtilities.ToRSAParameters(rsaPrivateCrtKeyParams);
            }
            else if (keyObject is RsaPrivateCrtKeyParameters rsaPrivateKey)
            {
                return DotNetUtilities.ToRSAParameters(rsaPrivateKey);
            }
            else
            {
                throw new ArgumentException("Unsupported key format");
            }
        }
    }

}

