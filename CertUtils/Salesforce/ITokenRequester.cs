using System;
namespace CertUtils.Salesforce
{
    public interface ITokenRequester
    {
        Task<string> RequestAccessTokenAsync(string jwtToken);
    }
}

