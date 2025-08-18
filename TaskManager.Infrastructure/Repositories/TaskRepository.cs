using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data;
using TaskManager.Application.Interfaces;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskRepository: ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task AddAsync(Domain.Entities.Task task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public async System.Threading.Tasks.Task<Domain.Entities.Task> GetByIdAsync(int id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.taskId == id);
        }
        
        public async System.Threading.Tasks.Task<List<Domain.Entities.Task>> GetAllTasksAsync(int userId)
        {
            return await _context.Tasks
                .Where(t => t.usrId == userId)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
