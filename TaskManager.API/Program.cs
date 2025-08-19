
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using TaskManager.Application.Handlers.Tasks;
using TaskManager.Application.Handlers.Users;
using TaskManager.Application.Interfaces;
using TaskManager.Data;
using TaskManager.Infrastructure.Repositories;
using TaskManager.Infrastructure;
using TaskManager.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TaskManager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //database connection için
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

            //CORS kullanarak frontende eriþim izni veriyoruz
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy => policy
                                      .WithOrigins("http://localhost:5173")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            //SMTP email sender için gerekli ayarlarý yapýyoruz
            builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SmtpOptions"));//load from config
            builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();//register the email sender service

            //repositoryleri ekliyoruz
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();

            //handlerlarý ekliyoruz
            //USERS
            builder.Services.AddScoped<RegisterUserHandler>();
            builder.Services.AddScoped<LoginUserHandler>();
            builder.Services.AddScoped<VerifyEmailHandler>();
            builder.Services.AddScoped<GetAllUsersHandler>();
            //TASKS
            builder.Services.AddScoped<GetTasksHandler>();
            builder.Services.AddScoped<CreateTaskHandler>();

            //password hasher service
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

            //binding jwt
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.Configure<JwtOptions>(
                builder.Configuration.GetSection("Jwt"));

            var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();//bu binding i birazdan kullanmak için yapýyoruz
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>//tells asp.net how to validate tokens
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(jwtOptions.Key))

                    };
                });
            builder.Services.AddAuthorization();//register authorization services

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowFrontend");

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
