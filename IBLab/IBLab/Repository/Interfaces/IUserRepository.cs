using IBLab.Models;

namespace IBLab.Repository.Interfaces
{
    public interface IUserRepository
    {
        void AddUser(User user);

        bool UserEmailExists(string email);

        bool UserUsernameExists(string username);

        User GetUserByUsername(string username);


        void DeleteUserByUsername(string username);

        void AddTempUser(TempUser tempUser);

        TempUser GetTempUserByUsername(string username);
    }
}
