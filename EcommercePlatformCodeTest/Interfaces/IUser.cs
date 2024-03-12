using EcommercePlatformCodeTest.Models;

namespace EcommercePlatformCodeTest.Interfaces
{
    public interface IUser
    {
        Task<User?> GetUserById(int? id);

        Task SaveUser(User user);

        Task UpdateUser(User user);

        Task<User?> AuthenticateLogin(string email, string password);
        Task<bool> GetUserByEmail(string email);
    }
}
