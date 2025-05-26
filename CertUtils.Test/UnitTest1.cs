using System.Threading.Tasks;
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
        var clientId = "3MVG9GCMQoQ6rpzR0ZxpRnvsA_pWCNS2PpUUNcJ4ujokj80WCNo7EBGx9pgRbzhqmUAuQM6_kJsMzQCEmf_sJ";
        var username = "richardla@vandewiele.test";
        var authEndpoint = "https://vandewiele-dev-ed.develop.my.salesforce.com";
        var aud = "https://test.salesforce.com";
        var privateKeyUrl = "https://gist.githubusercontent.com/ArticHand/e6a938c34e7102f2a9d72080242143e4/raw/a946ba4303a2813b0b4369bcba32dba81f1f41e2/gistfile1.txt";
        // Dependency Injection

        IPrivateKeyProvider privateKeyProvider = new UrlPrivateKeyProvider(privateKeyUrl);
        var pkey=privateKeyProvider.GetPrivateKeyAsync().Result;

        // Dependency Injection
        IJwtTokenGenerator jwtTokenGenerator = new JwtTokenGenerator(clientId, username,aud, authEndpoint);
        var jwt=jwtTokenGenerator.GenerateJwtToken(pkey);
        Assert.NotEmpty(jwt);
    }

        [Fact]
    public async Task GetSalesforceToken_True()
    {
        var clientId = "3MVG9GCMQoQ6rpzR0ZxpRnvsA_pWCNS2PpUUNcJ4ujokj80WCNo7EBGx9pgRbzhqmUAuQM6_kJsMzQCEmf_sJ";
        var username = "richardla@vandewiele.test";
        var authEndpoint = "https://vandewiele-dev-ed.develop.my.salesforce.com/services/oauth2/token";
        var aud = "https://test.salesforce.com";
        var privateKeyUrl = "https://gist.githubusercontent.com/ArticHand/e6a938c34e7102f2a9d72080242143e4/raw/a946ba4303a2813b0b4369bcba32dba81f1f41e2/gistfile1.txt";
        // Dependency Injection
        IPrivateKeyProvider privateKeyProvider = new UrlPrivateKeyProvider(privateKeyUrl);
        // Dependency Injection
        IJwtTokenGenerator jwtTokenGenerator = new JwtTokenGenerator(clientId, username,aud, authEndpoint);
        ITokenRequester requester=new SalesforceTokenRequester(authEndpoint);
        SalesforceTokenProvider provider =new SalesforceTokenProvider(privateKeyProvider,jwtTokenGenerator,requester);
        var sfToken=await provider.GetAccessTokenAsync();
        Assert.NotNull(sfToken);
        
    }
}

