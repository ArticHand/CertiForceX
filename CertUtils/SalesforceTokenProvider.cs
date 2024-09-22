using System;
using CertUtils.Cert;
using CertUtils.Salesforce;
using CertUtils.Token;

namespace CertUtils
{
    public class SalesforceTokenProvider
    {
        private readonly IPrivateKeyProvider _privateKeyProvider;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ITokenRequester _tokenRequester;

        public SalesforceTokenProvider(
            IPrivateKeyProvider privateKeyProvider,
            IJwtTokenGenerator jwtTokenGenerator,
            ITokenRequester tokenRequester)
        {
            _privateKeyProvider = privateKeyProvider;
            _jwtTokenGenerator = jwtTokenGenerator;
            _tokenRequester = tokenRequester;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            string privateKey = await _privateKeyProvider.GetPrivateKeyAsync();
            string jwtToken = _jwtTokenGenerator.GenerateJwtToken(privateKey);
            return await _tokenRequester.RequestAccessTokenAsync(jwtToken);
        }
    }

}

