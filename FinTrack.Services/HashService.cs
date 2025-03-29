using FinTrack.Services.Contracts;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace FinTrack.Services
{
    public class HashService : IHashService
    {
        private const char _separator = ':';
        private readonly ILogger _logger;

        public HashService(ILogger<HashService> logger)
        {
            _logger = logger;
        }

        public async Task<string> CreateHashPassword(string password)
        {
            _logger.LogInformation("HashService.CreateHashPassword started");

            byte[] salt;
            byte[] hash;
            byte[] key;

            if (password == null)
            {
                _logger.LogWarning("HashService.CreateHashPassword failed. Password is null.");
                throw new ArgumentNullException("Password is null.");
            }

            salt = RandomNumberGenerator.GetBytes(password.Length);

            using (SHA256 algoritm = SHA256.Create())
            {

                key = algoritm.ComputeHash(Encoding.UTF8.GetBytes(password));
                hash = algoritm.ComputeHash(Encoding.UTF8.GetBytes(string.Join(Convert.ToHexString(salt), Convert.ToHexString(key))));
            }

            _logger.LogInformation("HashService.CreateHashPassword completed");
            return string.Join(_separator, Convert.ToHexString(salt), Convert.ToHexString(hash));
        }

        public async Task<bool> VerifyHashedPassword(string hashPassword, string password)
        {
            _logger.LogInformation("HashService.VerifyHashedPassword started");

            byte[] hash;
            byte[] key;

            if (hashPassword == null)
            {
                _logger.LogWarning("HashService.VerifyHashedPassword failed. Hash is null.");
                throw new ArgumentNullException("Hash is null.");
            }

            if (password == null)
            {
                return false;
            }

            var array = hashPassword.Split(_separator);

            using (SHA256 algoritm = SHA256.Create())
            {

                key = algoritm.ComputeHash(Encoding.UTF8.GetBytes(password));
                hash = algoritm.ComputeHash(Encoding.UTF8.GetBytes(string.Join(array[0], Convert.ToHexString(key))));
            }

            _logger.LogInformation("HashService.VerifyHashedPassword completed");
            return array[1] == Convert.ToHexString(hash);
        }
    }
}
