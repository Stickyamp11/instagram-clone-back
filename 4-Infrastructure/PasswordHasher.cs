using Application;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 32;
        private const int KeySize = 64;
        private const int Iterations = 100;
        private const char Delimiter = ';';
        public string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);

            return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var salt = Convert.FromBase64String(hashedPassword.Split(Delimiter)[0]);
            var hash = Convert.FromBase64String(hashedPassword.Split(Delimiter)[1]);

            var newHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);

            return CryptographicOperations.FixedTimeEquals(hash, newHash);
        }
    }
}
