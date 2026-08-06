using System.Collections.Concurrent;
using MyEcomm.Identity.Api.Models;

namespace MyEcomm.Identity.Api.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<Guid, User> _users = new();

    public void Seed(IEnumerable<User> users)
    {
        foreach (var user in users)
        {
            _users[user.Id] = user;
        }
    }

    public User? GetByUsername(string username)
    {
        return _users.Values.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
    }

    public User Add(User user)
    {
        user.Id = Guid.NewGuid();
        user.CreatedAt = DateTime.UtcNow;
        _users[user.Id] = user;
        return user;
    }

    public bool UsernameExists(string username)
    {
        return GetByUsername(username) is not null;
    }
}
