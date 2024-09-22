using System;
using System.Text;

namespace CertUtils.Salesforce
{
    public class SalesforceTokenRequester : ITokenRequester
    {
        private readonly string _authEndpoint;

        public SalesforceTokenRequester(string authEndpoint)
        {
            _authEndpoint = authEndpoint;
        }

        public async Task<string> RequestAccessTokenAsync(string jwtToken)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _authEndpoint);
                var content = new StringContent($"grant_type=urn:ietf:params:oauth:grant-type:jwt-bearer&assertion={jwtToken}", Encoding.UTF8, "application/x-www-form-urlencoded");
                request.Content = content;

                var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error retrieving access token: {response.StatusCode}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent; // Parse the response if necessary
            }
        }
    }

}

