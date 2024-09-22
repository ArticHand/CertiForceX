using System;
namespace CertUtils.Token
{
    public interface IJwtTokenGenerator
    {
        string GenerateJwtToken(string privateKey);
    }
}

