using MyEcomm.Identity.Api.Models;

namespace MyEcomm.Identity.Api.Repositories;

public interface IUserRepository
{
    void Seed(IEnumerable<User> users);
    User? GetByUsername(string username);
    User Add(User user);
    bool UsernameExists(string username);
}
