using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Handlers.Users
{
    public class VerifyEmailHandler
    {
        private readonly IUserRepository _userRepo;

        public VerifyEmailHandler(IUserRepository _userRepo)
        {
            _userRepo = _userRepo;
        }

        public async System.Threading.Tasks.Task HandleAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token required.");//eğer catch block bulunamazsa ASP.NET Core bu hatayı otomatik olarak yakalar ve 400 Bad Request döner.
            }

            var user= await _userRepo.GetByVerificationTokenAsync(token);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid token.");
            }

            if(user.verificationTokenExpiration.HasValue && user.verificationTokenExpiration.Value < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Token expired.");
            }

            user.verified = true;
            user.verifiedAt = DateTime.UtcNow;
            //temizlik yapıyoruz
            user.verificationToken = null;
            user.verificationTokenExpiration = null;

            await _userRepo.SaveChangesAsync();
        }
    }
}
