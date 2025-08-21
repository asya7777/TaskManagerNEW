using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;

namespace TaskManager.Application.Handlers.Tasks
{
    public class FinishTaskHandler
    {
        private readonly ITaskRepository _taskRepo;

        public FinishTaskHandler(ITaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }

        public async System.Threading.Tasks.Task HandleAsync(int id, bool isFinished)
        {
            var task = await _taskRepo.GetByIdAsync(id);

            task.isFinished = isFinished;
            await _taskRepo.SaveChangesAsync();
        }
    }
}
