# CertiForce

A .NET library for secure Salesforce authentication using JWT Bearer Token Flow with private key authentication. This library provides a more secure alternative to username/password authentication by leveraging X.509 certificates for server-to-server authentication.

## Features

- Secure Salesforce authentication using JWT Bearer Token Flow
- Support for PKCS#1 and PKCS#8 private key formats
- Async/await support for all operations
- Extensible architecture with interfaces for custom implementations
- Built with .NET 8.0

## Installation

### NuGet Package Manager
```bash
Install-Package CertiForce
```

### .NET CLI
```bash
dotnet add package CertiForce
```

## Prerequisites

- .NET 8.0 SDK or later
- A Salesforce Connected App with JWT Bearer Flow enabled
- A valid X.509 certificate with private key (PKCS#1 or PKCS#8 format)
- Salesforce User with the necessary permissions

## Usage

### Basic Usage

```csharp
using CertUtils;
using CertUtils.Cert;
using CertUtils.Salesforce;
using CertUtils.Token;

// Initialize the providers
var privateKeyProvider = new UrlPrivateKeyProvider("https://your-domain.com/private.key");
var jwtTokenGenerator = new JwtTokenGenerator(
    "your-client-id",
    "your-salesforce-username",
    "https://login.salesforce.com"
);
var tokenRequester = new SalesforceTokenRequester("https://login.salesforce.com");

// Create the token provider
var tokenProvider = new SalesforceTokenProvider(
    privateKeyProvider,
    jwtTokenGenerator,
    tokenRequester
);

// Get the access token
try
{
    string accessToken = await tokenProvider.GetAccessTokenAsync();
    Console.WriteLine($"Access Token: {accessToken}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

### Custom Implementations

You can implement the following interfaces to customize the behavior:

- `IPrivateKeyProvider`: Provide private key from custom sources
- `IJwtTokenGenerator`: Custom JWT token generation
- `ITokenRequester`: Custom token request handling

## Configuration

### Salesforce Connected App Setup

1. In Salesforce Setup, navigate to **App Manager**
2. Create a new Connected App
3. Enable **Use digital signatures** and upload your certificate
4. Enable **JWT Bearer Flow**
5. Add the following OAuth scopes:
   - Access and manage your data (api)
   - Perform requests on your behalf at any time (refresh_token, offline_access)

## Error Handling

The library throws exceptions for various error conditions. Always wrap token acquisition in try-catch blocks and handle exceptions appropriately.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Built with ❤️ by Richard Lawidjaja
- Uses [Portable.BouncyCastle](https://www.nuget.org/packages/Portable.BouncyCastle/) for cryptographic operations
- Inspired by Salesforce's JWT Bearer Token Flow
