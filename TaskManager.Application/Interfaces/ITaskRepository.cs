using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace TaskManager.Application.Interfaces
{
    public interface ITaskRepository
    {
        System.Threading.Tasks.Task AddAsync(Domain.Entities.Task task);//change in memory
        System.Threading.Tasks.Task<Domain.Entities.Task> GetByIdAsync(int id);
        System.Threading.Tasks.Task<List<Domain.Entities.Task>> GetAllTasksAsync(int userId);
        System.Threading.Tasks.Task SaveChangesAsync();//commit changes to the database
    }
}
