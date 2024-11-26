using FluentEmail.Core;
using IBLab.Models;
using IBLab.Repository.Impl;
using IBLab.Repository.Interfaces;
using IBLab.Service.interfaces;
using MimeKit;
using MailKit;
using System.Security.Cryptography;
using MailKit.Net.Smtp;


namespace IBLab.Service.impl
{
    public class UserServiceImpl : IUserService
    {

        public readonly IUserRepository _userRepository;

        public UserServiceImpl(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        

        public void ValidateEmail(string email)
        {
            if (_userRepository.UserEmailExists(email))
            {
                throw new Exception("A user with this email already exists, proceed to the login page if have an account.");
            }
        }

        public void ValidateUsername(string username)
        {
            if (_userRepository.UserUsernameExists(username))
            {
                throw new Exception("This username is already taken, please choose a new one.");
            }
        }

        public bool ValidateLogin(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user == null)
            {
                throw new Exception("Wrong username! If you dont have an account proceed to the registration page.");
            }

            if(!ValidatePassword(password, user))
            {
                throw new Exception("Wrong password!");
            }

            return true;
        }

        public bool ValidatePassword(string password, User user)
        {
            const int HashSize = 32;
            const int Iterations = 100000;
            HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;
            string[] parts = user.Password.Split("-");
            byte[] hash = Convert.FromHexString(parts[0]);
            byte[] salt = Convert.FromHexString(parts[1]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }


        public string HashPassword(string password)
        {
            const int SaltSize = 16;
            const int HashSize = 32;
            const int Iterations = 100000;
            HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);
            
            return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        }

        public void SendEmail(string email)
        {
            const string emailAddress = "ivaniblabs@gmail.com";
            const string emailPassword = "kvwz zwhq ntxi uqrr";
            //var otp = GenerateOTP();

            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Ivan", "ivaniblabs@gmail.com"));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Email Verification";
            message.Body = new TextPart("plain")
            {
                Text = $"Your verification code is .\nThis code will expire in 3 minutes!"
            };

            SmtpClient client = new SmtpClient();
           
            try
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(emailAddress, emailPassword);
                client.Send(message);

            }
            catch (Exception)
            {
                throw new Exception("Email verification couldnt be sent!");
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }


        public void CreateUser(string username)
        {

            var tempUser = _userRepository.GetTempUserByUsername(username);


            var user = new User
            {
                Email = tempUser.Email,
                Username = tempUser.Username,
                Password = tempUser.Password
            };

            
            _userRepository.AddUser(user);
        }

        public void CreateTempUser(string email, string username, string password)
        {
            ValidateEmail(email);
            ValidateUsername(username);

            var tempUser = new TempUser
            {
                Email = email,
                Username = username,
                Password = HashPassword(password),
                Code = GenerateOTP(),
                ExpirationTime = DateTime.UtcNow.AddMinutes(3)
            };


            _userRepository.AddTempUser(tempUser);
        }


        public string GenerateOTP()
        {
            var random = new Random();
            return string.Join("", Enumerable.Range(0, 6).Select(_ => random.Next(0, 10).ToString()));
        }


        public bool ValidateOTP(string username, string code)
        {
            var user = _userRepository.GetTempUserByUsername(username);

            if(user == null)
            {
                throw new Exception("Verification time expired, return to the registration page");
            }

            if(user.Code != code || user.ExpirationTime < DateTime.UtcNow)
            {
                throw new Exception("The code is wrong or expired!");
            }

            return true;
        }
    }
}
