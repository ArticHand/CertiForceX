using System.Security.Cryptography;
using System.Threading.Tasks;
using CertUtils.Cert;
using CertUtils.Salesforce;
using CertUtils.Token;
using Xunit;

namespace CertUtils.Test;

public class UnitTest1 : IAsyncLifetime
{
    private IPrivateKeyProvider _privateKeyProvider;
    private IJwtTokenGenerator _jwtTokenGenerator;
    private ITokenRequester _tokenRequester;
    private SalesforceTokenProvider _tokenProvider;

    private const string ClientId = "3MVG9GCMQoQ6rpzR0ZxpRnvsA_pWCNS2PpUUNcJ4ujokj80WCNo7EBGx9pgRbzhqmUAuQM6_kJsMzQCEmf_sJ";
    private const string Username = "richardla@vandewiele.test";
    private const string AuthEndpoint = "https://vandewiele-dev-ed.develop.my.salesforce.com";
    private const string Aud = "https://test.salesforce.com";
    private const string PrivateKeyUrl = "https://gist.githubusercontent.com/ArticHand/e6a938c34e7102f2a9d72080242143e4/raw/a946ba4303a2813b0b4369bcba32dba81f1f41e2/gistfile1.txt";

    public async Task InitializeAsync()
    {
        _privateKeyProvider = new UrlPrivateKeyProvider(PrivateKeyUrl);
        _jwtTokenGenerator = new JwtTokenGenerator(ClientId, Username, Aud, AuthEndpoint);
        _tokenRequester = new SalesforceTokenRequester(AuthEndpoint);
        _tokenProvider = new SalesforceTokenProvider(_privateKeyProvider, _jwtTokenGenerator, _tokenRequester);
        
        // Warm up the provider
        await _privateKeyProvider.GetPrivateKeyAsync();
    }

    public Task DisposeAsync()
    {
        // No explicit disposal needed as we're not creating any disposable resources
        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetPrivateKeyReturn_True()
    {
        // Act
        var pkey = await _privateKeyProvider.GetPrivateKeyAsync();
        
        // Assert
        Assert.NotNull(pkey);
        Assert.NotEmpty(pkey);
    }

    [Fact]
    public async Task GetJwtString_True()
    {
        // Arrange
        var pkey = await _privateKeyProvider.GetPrivateKeyAsync();

        // Act
        var jwt = _jwtTokenGenerator.GenerateJwtToken(pkey);
        
        // Assert
        Assert.NotNull(jwt);
        Assert.NotEmpty(jwt);
    }

    /* [Fact]
    public async Task GetSalesforceToken_True()
    {
        // Act
        var sfToken = await _tokenProvider.GetAccessTokenAsync();
        
        // Assert
        Assert.NotNull(sfToken);
    } */
}