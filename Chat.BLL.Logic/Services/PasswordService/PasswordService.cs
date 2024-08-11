using Microsoft.AspNetCore.Identity;

namespace HireWise.BLL.Logic.Services
{
    public class PasswordService
    {
        private readonly PasswordHasher _passwordHasher;

        public PasswordService(PasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(password);
        }

        public bool VerifyPassword(string hashedPassword, string passwordToCheck)
        {
            return _passwordHasher.VerifyPassword(hashedPassword, passwordToCheck);
        }
    }
}
