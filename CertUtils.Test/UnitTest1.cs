using CertUtils.Cert;
using CertUtils.Salesforce;
using CertUtils.Token;

namespace CertUtils.Test;

public class UnitTest1
{
    [Fact]
    public void GetPrivateKeyReturn_True()
    {
        var privateKeyUrl = "https://pastebin.com/raw/pdykvseW";
        // Dependency Injection
        IPrivateKeyProvider privateKeyProvider = new UrlPrivateKeyProvider(privateKeyUrl);
        var pkey=privateKeyProvider.GetPrivateKeyAsync().Result;
        Assert.NotEmpty(pkey);

    }

    [Fact]
    public void GetJwtString_True()
    {
        var clientId = "yoursalesforceid";
        var username = "richardla@abc.test";
        var authEndpoint = "https://yoursalesforceurl";
        var aud = "https://test.salesforce.com";
        var privateKeyUrl = "https://pastebin.com/raw/pdykvseW";
        // Dependency Injection

        IPrivateKeyProvider privateKeyProvider = new UrlPrivateKeyProvider(privateKeyUrl);
        var pkey=privateKeyProvider.GetPrivateKeyAsync().Result;

        // Dependency Injection
        IJwtTokenGenerator jwtTokenGenerator = new JwtTokenGenerator(clientId, username,aud, authEndpoint);
        var jwt=jwtTokenGenerator.GenerateJwtToken(pkey);
        Assert.NotEmpty(jwt);
    }
}

