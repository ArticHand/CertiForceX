using CertUtils.Cert;
using CertUtils.Salesforce;
using CertUtils.Token;

namespace CertUtils.Test;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var clientId = "Salesforce ClientId";
        var username = "salesforce username";
        var authEndpoint = "authorization url";
        var privateKeyUrl = "path to the private key";
        var aud = "salesforce Audience";

        // Dependency Injection
        IPrivateKeyProvider privateKeyProvider = new UrlPrivateKeyProvider(privateKeyUrl);
        IJwtTokenGenerator jwtTokenGenerator = new JwtTokenGenerator(clientId, username,aud, authEndpoint);
        ITokenRequester tokenRequester = new SalesforceTokenRequester(authEndpoint);

        // High-level class that uses the abstractions
        var tokenProvider = new SalesforceTokenProvider(privateKeyProvider, jwtTokenGenerator, tokenRequester);

        string accessToken =  tokenProvider.GetAccessTokenAsync().Result;
        Console.WriteLine($"Salesforce Access Token: {accessToken}");

    }
}

