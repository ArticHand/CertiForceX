using System;
namespace CertUtils.Cert
{
    public interface IPrivateKeyProvider
    {
        Task<string> GetPrivateKeyAsync();
    }
}

