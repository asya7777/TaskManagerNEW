
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
