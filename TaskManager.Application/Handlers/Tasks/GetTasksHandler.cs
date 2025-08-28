using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;

namespace TaskManager.Application.Handlers.Tasks
{
    public class GetTasksHandler
    {
        private readonly ITaskRepository _taskRepo;

        public GetTasksHandler(ITaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }

        public async System.Threading.Tasks.Task<List<object>> HandleAsync(int userId)//gerçekten liste halinde görmek istediğimiz için IEnumerable yerine List dedik
        {
            var tasks = await _taskRepo.GetAllTasksAsync(userId);//await kullanmazsan üzerine select kullanamazsın, önce liste olması lazım

            return tasks.Select(t => new {t.taskId, t.taskName, t.taskDeadline, t.taskDescription, t.usrId, t.isFinished,
                Tags=t.Tags.Select(tag => new {tag.tagId, tag.tagName}).ToList()}).Cast<object>().ToList();
        }
    }
}
