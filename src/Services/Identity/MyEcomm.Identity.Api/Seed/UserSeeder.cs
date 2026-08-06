using MyEcomm.Identity.Api.Models;
using MyEcomm.Identity.Api.Repositories;

namespace MyEcomm.Identity.Api.Seed;

public static class UserSeeder
{
    public static void SeedUsers(IUserRepository repository)
    {
        var users = new List<User>
        {
            new() { Id = Guid.NewGuid(), Username = "demo", PasswordHash = BCrypt.Net.BCrypt.HashPassword("demo123"), CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Username = "alice", PasswordHash = BCrypt.Net.BCrypt.HashPassword("alice123"), CreatedAt = DateTime.UtcNow },
        };

        repository.Seed(users);
    }
}
