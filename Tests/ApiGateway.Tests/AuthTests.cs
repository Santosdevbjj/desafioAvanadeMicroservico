using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Shared.Auth;

namespace ApiGateway.Tests;

public class AuthTests
{
    [Fact]
    public void Deve_Gerar_Token_JWT()
    {
        var inMemorySettings = new Dictionary<string, string> {
            {"Jwt:Key", "supersecretkey123456789"},
            {"Jwt:Issuer", "TestIssuer"},
            {"Jwt:Audience", "TestAudience"}
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var jwtService = new JwtTokenService(configuration);

        var token = jwtService.GenerateToken("user1", "admin");

        Assert.False(string.IsNullOrEmpty(token));
    }
}
