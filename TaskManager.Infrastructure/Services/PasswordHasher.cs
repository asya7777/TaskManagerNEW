using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;//for HMACSHA512
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;

namespace TaskManager.Infrastructure.Services
{
    public class PasswordHasher: IPasswordHasher
    {
        public (byte[] hash, byte[] salt) HashPassword(string password)
        {
            using var hmac = new HMACSHA512();
            var salt = hmac.Key; //hmac random olarak bir salt üretiyor
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));//password'u byte array'e çevirip hashliyoruz

            return (hash, salt);
        }
        public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));//input alınan passwordü hashliyoruz

            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
