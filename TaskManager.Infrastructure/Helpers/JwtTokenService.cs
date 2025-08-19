
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;//for user info
using Microsoft.Extensions.Configuration;//to read appsettings.json
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using TaskManager.Domain.Entities;//for IOptions

public class JwtOptions
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public int ExpireMinutes { get; set; }
}


namespace TaskManager.Infrastructure.Services
{
    public class JwtTokenService:IJwtTokenService
    {
        private readonly JwtOptions _jwtOptions;

        public JwtTokenService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            Console.WriteLine($"JWT Key: {_jwtOptions.Key}");

        }

        public string GenerateToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_jwtOptions.Key);//yazdığım key i byte lara çeviriyor

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.usrId.ToString()),
                new Claim(ClaimTypes.Email, user.email.ToString()),
                new Claim(ClaimTypes.Role, user.userRole.ToString())//authorization için
            };

            var creds = new SigningCredentials(//signature için
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256);//kullanılan algoritma

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpireMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
