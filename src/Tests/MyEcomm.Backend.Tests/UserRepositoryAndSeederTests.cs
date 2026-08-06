using MyEcomm.Identity.Api.Repositories;
using MyEcomm.Identity.Api.Seed;

namespace MyEcomm.Backend.Tests;

public class UserRepositoryAndSeederTests
{
    [Fact]
    public void Add_AndLookup_ShouldWorkCaseInsensitive()
    {
        var repo = new InMemoryUserRepository();

        var created = repo.Add(new MyEcomm.Identity.Api.Models.User
        {
            Username = "DemoUser",
            PasswordHash = "hash"
        });

        var found = repo.GetByUsername("demouser");

        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.NotNull(found);
        Assert.True(repo.UsernameExists("DEMOUSER"));
        Assert.False(repo.UsernameExists("missing"));
    }

    [Fact]
    public void SeedUsers_ShouldAddDefaultUsersWithBCryptHashes()
    {
        var repo = new InMemoryUserRepository();

        UserSeeder.SeedUsers(repo);

        var demo = repo.GetByUsername("demo");
        var alice = repo.GetByUsername("alice");

        Assert.NotNull(demo);
        Assert.NotNull(alice);
        Assert.True(BCrypt.Net.BCrypt.Verify("demo123", demo!.PasswordHash));
        Assert.True(BCrypt.Net.BCrypt.Verify("alice123", alice!.PasswordHash));
    }
}