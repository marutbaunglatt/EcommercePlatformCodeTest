using EcommercePlatformCodeTest.Data;
using EcommercePlatformCodeTest.Interfaces;
using EcommercePlatformCodeTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace EcommercePlatformCodeTest.Repositoriies
{
    public class UserRepo : IUser
    {
        private readonly ApplicationDbContext _context;
        public UserRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateLogin(string email, string password)
        {
            var user = await _context.Users.AsNoTracking()
                 .SingleOrDefaultAsync(x => x.Email == email);

            if (user != null)
            {
                bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, user.Password);
                if (isPasswordCorrect)
                    return user;
            }
            return null;
        }

        public async Task<bool> GetUserByEmail(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserById(int? id)
        {
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task SaveUser(User user)
        {
            user.Password = HashPassword(user.Password);
            if (user.Id == 0)
            {
                user.CreatedAt = DateTime.Now;
                await _context.Users.AddAsync(user);
            }
            else
            {
                user.UpdatedAt = DateTime.Now;
                _context.Users.Update(user);
            }
            await _context.SaveChangesAsync();
        }

        public Task UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
