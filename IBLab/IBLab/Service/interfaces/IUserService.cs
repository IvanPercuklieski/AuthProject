using IBLab.Models;
using IBLab.Repository;

namespace IBLab.Service.interfaces
{
    public interface IUserService
    {
        void CreateUser(string username);

        void ValidateEmail(string email);

        void ValidateUsername(string username);

        bool ValidateLogin(string username, string password);

        bool ValidatePassword(string password, User user);

        void SendEmail(string email, string code);

        string HashPassword(string password);
        void CreateTempUser(string email, string username, string password);

        bool ValidateOTP(string username, string code);

        string GenerateOTP();

        void TFA(string username);


    }
}
