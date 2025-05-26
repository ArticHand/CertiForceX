using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using CertUtils.Token.KeyFormats;

namespace CertUtils.Token
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly string _clientId;
        private readonly string _username;
        private readonly string _aud;
        private readonly string _authEndpoint;
        private readonly PrivateKeyParserFactory _parserFactory;

        public JwtTokenGenerator(string clientId, string username, string aud, string authEndpoint)
            : this(clientId, username, aud, authEndpoint, new PrivateKeyParserFactory())
        {
        }

        // Constructor for testing with a custom parser factory
        internal JwtTokenGenerator(
            string clientId, 
            string username, 
            string aud, 
            string authEndpoint,
            PrivateKeyParserFactory parserFactory)
        {
            _clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            _username = username ?? throw new ArgumentNullException(nameof(username));
            _aud = aud ?? throw new ArgumentNullException(nameof(aud));
            _authEndpoint = authEndpoint ?? throw new ArgumentNullException(nameof(authEndpoint));
            _parserFactory = parserFactory ?? throw new ArgumentNullException(nameof(parserFactory));
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
            try
            {
                var parser = _parserFactory.GetParser(privateKey);
                return parser.Parse(privateKey);
            }
            catch (Exception ex) when (ex is not ArgumentException)
            {
                throw new ArgumentException($"Failed to parse private key. Error: {ex.Message}", ex);
            }
        }
    }

}

