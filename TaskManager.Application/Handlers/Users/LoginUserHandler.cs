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
    public class LoginUserHandler
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginUserHandler(IUserRepository userRepo, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async System.Threading.Tasks.Task<object> HandleAsync(LoginDTO dto)//returns an object
        {
            var user = await _userRepo.GetByEmailAsync(dto.email);//await kullandık çünkü bir database operation
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email.");
            }

            if(!_passwordHasher.VerifyPassword(dto.password, user.passwordHash, user.passwordSalt))
            {
                throw new UnauthorizedAccessException("Invalid password.");
            }
            if (!user.verified)
            {
                throw new UnauthorizedAccessException("Please verify your email before logging in.");
            }

            var token = _jwtTokenService.GenerateToken(user);

            return new {userId=user.usrId, token=token, firstName = user.firstName, role= user.userRole};
        }
    }
}
