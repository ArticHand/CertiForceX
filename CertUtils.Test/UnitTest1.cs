using System.Security.Cryptography;
using System.Threading.Tasks;
using CertUtils.Cert;
using CertUtils.Salesforce;
using CertUtils.Token;
using Xunit;

namespace CertUtils.Test;

public class UnitTest1 : IDisposable
{
    private readonly RSA _rsa;
    private readonly IPrivateKeyProvider _privateKeyProvider;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ITokenRequester _tokenRequester;
    private readonly SalesforceTokenProvider _tokenProvider;

    private const string ClientId = "3MVG9GCMQoQ6rpzR0ZxpRnvsA_pWCNS2PpUUNcJ4ujokj80WCNo7EBGx9pgRbzhqmUAuQM6_kJsMzQCEmf_sJ";
    private const string Username = "richardla@vandewiele.test";
    private const string AuthEndpoint = "https://vandewiele-dev-ed.develop.my.salesforce.com";
    private const string Aud = "https://test.salesforce.com";
    private const string PrivateKeyUrl = "https://gist.githubusercontent.com/ArticHand/e6a938c34e7102f2a9d72080242143e4/raw/a946ba4303a2813b0b4369bcba32dba81f1f41e2/gistfile1.txt";

    public UnitTest1()
    {
        // Initialize RSA with a new key for each test
        _rsa = RSA.Create(2048);
        _privateKeyProvider = new UrlPrivateKeyProvider(PrivateKeyUrl);
        _jwtTokenGenerator = new JwtTokenGenerator(ClientId, Username, Aud, AuthEndpoint);
        _tokenRequester = new SalesforceTokenRequester(AuthEndpoint);
        _tokenProvider = new SalesforceTokenProvider(_privateKeyProvider, _jwtTokenGenerator, _tokenRequester);
    }

    [Fact]
    public void GetPrivateKeyReturn_True()
    {
        // Act
        var pkey = _privateKeyProvider.GetPrivateKeyAsync().Result;
        
        // Assert
        Assert.NotNull(pkey);
        Assert.NotEmpty(pkey);
    }

    [Fact]
    public void GetJwtString_True()
    {
        // Arrange
        var pkey = _privateKeyProvider.GetPrivateKeyAsync().Result;

        // Act
        var jwt = _jwtTokenGenerator.GenerateJwtToken(pkey);
        
        // Assert
        Assert.NotNull(jwt);
        Assert.NotEmpty(jwt);
    }

    [Fact]
    public async Task GetSalesforceToken_True()
    {
        // Act
        var sfToken = await _tokenProvider.GetAccessTokenAsync();
        
        // Assert
        Assert.NotNull(sfToken);
    }

    public void Dispose()
    {
        _rsa?.Dispose();
    }
}