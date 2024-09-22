using System;
namespace CertUtils.Cert
{
    public class UrlPrivateKeyProvider : IPrivateKeyProvider
    {
        private readonly string _privateKeyUrl;

        public UrlPrivateKeyProvider(string privateKeyUrl)
        {
            _privateKeyUrl = privateKeyUrl;
        }

        public async Task<string> GetPrivateKeyAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_privateKeyUrl);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Unable to download private key from URL: {response.StatusCode}");
                }
                return await response.Content.ReadAsStringAsync();
            }
        }
    }

}

