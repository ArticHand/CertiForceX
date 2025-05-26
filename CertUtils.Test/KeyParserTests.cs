using System;
using System.Security.Cryptography;
using System.Text;
using Xunit;
using CertUtils.Token;
using CertUtils.Token.KeyFormats;

namespace CertUtils.Test
{
    public class KeyParserTests
    {
        [Fact]
        public void Should_Parse_PKCS1_Key()
        {
            // Arrange
            var pkcs1Key = @"-----BEGIN RSA PRIVATE KEY-----
MIIBOgIBAAJBAKj34GkxFhD90vcNLYLInFEX6Ppy1tPf9Cnzj4p4WGeKLs1Pt8Qu
KUpRKfFLfRYC9AIKjbJTWit+CqvjWYzvQwECAwEAAQJAIJLixBy2qpFoS4DSmoEm
o3qGy0t6z09AIJtH+5OeRV1be+N4cDYJKffGzDa88vQENZiRm0GRq6a+HPGQMd2k
TQIhAKMSvzIBnni7ot/OSie2TmJLY4SwTQAevXysE2RbFDYdAiEBCUEaRQnMnbp7
9mxDXDf6AU0cN/RPBjb9qSHDcWZHGzUCIG2Es59z8ugGrDY+pxLQnwfotadxd+Uy
v/Ow5T0q5gIJAiEAyS4RaI9YG8EWx/2w0T67ZUVAw8eOMB6BIUg0Xcu+3okCIBOs
/5OiPgoTdSy7bcF9IGpSE8ZgGKzgYQVZeN97YE00
-----END RSA PRIVATE KEY-----";
            
            // Act & Assert
            TestKeyParsing(pkcs1Key);
        }

        [Fact]
        public void Should_Parse_PKCS8_Key()
        {
            // Arrange
            var pkcs8Key = @"-----BEGIN PRIVATE KEY-----
MIIBVgIBADANBgkqhkiG9w0BAQEFAASCAUAwggE8AgEAAkEAq7BFUpkGp3+LQmlQ
Yx2eqzDV+xeG8kx/sQFV18S5JhzGeIJNA72wSeukEPojtqUyX2J0CciPBh7eqclQ
2zpAswIDAQABAkAgisq4+zRdrzkwH1ITV1vpytnkO/NiHcnePQiOW0VUybPyHoGM
/jf75C5xET7ZQpBe5kx5VHsPZj0CBb3b+wSRAiEA2mPWCBytosIU/ODRfq6EiV04
lt6waE7I2uSPqIC20LcCIQDJQYIHQII+3YaPqyhGgqMexuuuGx+lDKD6/Fu/JwPb
5QIhAKthiYcYKlL9h8bjDsQhZDUACPasjzdsDEdq8inDyLOFAiEAmCr/tZwA3qeA
ZoBzI10DGPIuoKXBd3nk/eBxPkaxlEECIQCNymjsoI7GldtujVnr1qT+3yedLfHK
srDVjIT3LsvTqw==
-----END PRIVATE KEY-----";
            
            // Act & Assert
            TestKeyParsing(pkcs8Key);
        }

        [Fact]
        public void Should_Throw_For_Invalid_Key()
        {
            // Arrange
            var invalidKey = "invalid-key";
            var factory = new PrivateKeyParserFactory();

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => factory.GetParser(invalidKey));
        }

        private static void TestKeyParsing(string privateKey)
        {
            // Arrange
            var factory = new PrivateKeyParserFactory();
            var parser = factory.GetParser(privateKey);

            // Act
            var rsaParams = parser.Parse(privateKey);

            // Assert
            Assert.NotNull(rsaParams);
            Assert.NotNull(rsaParams.Modulus);
            Assert.NotNull(rsaParams.Exponent);
            Assert.NotNull(rsaParams.D);
        }
    }
}
