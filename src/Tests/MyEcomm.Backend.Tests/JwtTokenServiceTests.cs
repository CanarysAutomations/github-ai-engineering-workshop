using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using MyEcomm.Identity.Api.Models;
using MyEcomm.Identity.Api.Services;

namespace MyEcomm.Backend.Tests;

public class JwtTokenServiceTests
{
    [Fact]
    public void GenerateToken_ShouldIncludeStandardClaimsAndConfigurationValues()
    {
        var settings = new Dictionary<string, string?>
        {
            ["Jwt:SigningKey"] = "this-is-a-very-long-signing-key-for-tests-123456",
            ["Jwt:Issuer"] = "myecomm-tests",
            ["Jwt:Audience"] = "myecomm-clients",
            ["Jwt:AccessTokenMinutes"] = "30"
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        var service = new JwtTokenService(configuration);
        var user = new User { Id = Guid.NewGuid(), Username = "demo" };

        var (token, expiresAt) = service.GenerateToken(user);

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        Assert.False(string.IsNullOrWhiteSpace(token));
        Assert.Equal("myecomm-tests", jwt.Issuer);
        Assert.Contains("myecomm-clients", jwt.Audiences);
        Assert.Equal(user.Id.ToString(), jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
        Assert.Equal("demo", jwt.Claims.First(c => c.Type == "username").Value);
        Assert.True(expiresAt > DateTime.UtcNow.AddMinutes(20));
        Assert.True(expiresAt <= DateTime.UtcNow.AddMinutes(31));
    }

    [Fact]
    public void GenerateToken_ShouldFallbackTo60Minutes_WhenLifetimeMissing()
    {
        var settings = new Dictionary<string, string?>
        {
            ["Jwt:SigningKey"] = "this-is-a-very-long-signing-key-for-tests-abcdef",
            ["Jwt:Issuer"] = "issuer",
            ["Jwt:Audience"] = "audience"
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        var service = new JwtTokenService(configuration);

        var (_, expiresAt) = service.GenerateToken(new User { Username = "demo" });

        Assert.True(expiresAt > DateTime.UtcNow.AddMinutes(50));
        Assert.True(expiresAt <= DateTime.UtcNow.AddMinutes(61));
    }

    [Fact]
    public void GenerateToken_ShouldThrowFormatException_WhenLifetimeIsInvalid()
    {
        var settings = new Dictionary<string, string?>
        {
            ["Jwt:SigningKey"] = "this-is-a-very-long-signing-key-for-tests-xyz",
            ["Jwt:Issuer"] = "issuer",
            ["Jwt:Audience"] = "audience",
            ["Jwt:AccessTokenMinutes"] = "not-a-number"
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        var service = new JwtTokenService(configuration);

        Assert.Throws<FormatException>(() => service.GenerateToken(new User { Username = "demo" }));
    }
}