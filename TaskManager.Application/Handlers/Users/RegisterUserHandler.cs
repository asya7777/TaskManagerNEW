using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Handlers.Users
{
    public class RegisterUserHandler
    {
        private readonly IUserRepository _userRepo;
        private readonly IEmailSender _emailSender;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserHandler(IUserRepository userRepo, IEmailSender emailSender, IPasswordHasher passwordHasher)
        {
            _userRepo = userRepo;
            _emailSender = emailSender;
            _passwordHasher = passwordHasher;
        }

        public async System.Threading.Tasks.Task HandleAsync(RegisterDTO dto)
        {
            var (hash, salt) = _passwordHasher.HashPassword(dto.password);//şifreyi hashle

            var newUser = new User
            {
                email = dto.email,
                firstName = dto.firstName,
                lastName = dto.lastName,
                passwordHash = hash,
                passwordSalt = salt,
                createdAt = DateTime.UtcNow,
                updatedAt = DateTime.UtcNow,
                userRole = dto.userRole,

                //token oluşturuyoruz
                verificationToken = Guid.NewGuid().ToString(),
                verificationTokenExpiration = DateTime.UtcNow.AddDays(1)//token 1 gün geçerli olacak 
            };

            await _userRepo.AddAsync(newUser);// kullanıcıyı veritabanına ekle
            await _userRepo.SaveChangesAsync();//commit to db

            ////email gönderme kısmı
            //var verificationUrl = $"https://localhost:5082/api/User/verify?token={newUser.verificationToken}";
            //var body = $@"<h2>Welcome, {newUser.firstName}!</h2>
            //              <p>Thank you for registering. Please verify your email by clicking the link below:</p>
            //              <a href='{verificationUrl}'>Verify Email</a>
            //              <p>This link expires in 24 hours.</p>";

            //await _emailSender.SendAsync(newUser.email, "Email Verification", body);
        }
    }
}
