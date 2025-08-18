using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.Interfaces
{
    public interface IPasswordHasher//password hashing logic buraya
    {
        (byte[] hash, byte[] salt) HashPassword(string password);//tuple return leyen fonksiyon
        bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
