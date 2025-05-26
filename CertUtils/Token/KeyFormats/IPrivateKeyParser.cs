using System.Security.Cryptography;

namespace CertUtils.Token.KeyFormats
{
    public interface IPrivateKeyParser
    {
        bool CanParse(string privateKey);
        RSAParameters Parse(string privateKey);
    }
}
