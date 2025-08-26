using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface IUserRepository
    {
        System.Threading.Tasks.Task AddAsync(User user);//change in memory
        System.Threading.Tasks.Task<User?> GetByEmailAsync(string email);
        System.Threading.Tasks.Task<User?> GetByIdAsync(int id);
        System.Threading.Tasks.Task<User?> GetByVerificationTokenAsync(string token);//for email verification
        System.Threading.Tasks.Task<List<User>> GetAllUsersAsync();
        System.Threading.Tasks.Task SaveChangesAsync();//commit changes to the database
        System.Threading.Tasks.Task DeleteAsync(User user);
    }
}
