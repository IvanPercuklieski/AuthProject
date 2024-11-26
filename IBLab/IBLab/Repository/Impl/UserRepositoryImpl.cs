using IBLab.Data;
using IBLab.Models;
using IBLab.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IBLab.Repository.Impl
{
    public class UserRepositoryImpl : IUserRepository
    {
        public readonly IBLabContext _context;

        public UserRepositoryImpl(IBLabContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public bool UserEmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool UserUsernameExists(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public User GetUserByUsername(string username) 
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }


        public void DeleteUserByUsername(string code)
        {
            _context.Users.Remove(_context.Users.FirstOrDefault(c => c.Username == code));
            _context.SaveChanges();
        }

        public void AddTempUser(TempUser tempUser)
        {
            _context.TempUsers.Add(tempUser);
            _context.SaveChanges();
        }

        public TempUser GetTempUserByUsername(string username)
        {
            return _context.TempUsers.FirstOrDefault(u => u.Username == username);
        }
    }
}
