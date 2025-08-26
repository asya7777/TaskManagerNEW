
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Handlers.Tasks
{
    public class CreateTaskHandler
    {
        private readonly ITaskRepository _taskRepo;
        private readonly ITagRepository _tagRepo;
        public CreateTaskHandler(ITaskRepository taskRepo, ITagRepository tagRepo)
        {
            _taskRepo = taskRepo;
            _tagRepo = tagRepo;
        }

        public async System.Threading.Tasks.Task HandleAsync(CreateTaskDTO dto)
        {
            var newTask = new Domain.Entities.Task
            {
                taskName = dto.taskName,
                taskDescription = dto.taskDescription,
                taskDeadline = dto.taskDeadline,
                usrId = dto.usrId,
                Tags=new List<Tag>()
            };

            foreach (var tagName in dto.Tags)
            {
                var existingTag = await _tagRepo.GetTagsByNameAsync(tagName);
                if (existingTag != null)
                {
                    newTask.Tags.Add(existingTag);
                }
                else
                {
                    var newTag = new Tag { tagName = tagName };
                    await _tagRepo.AddAsync(newTag);
                    newTask.Tags.Add(newTag);
                }
            }

            await _taskRepo.AddAsync(newTask);
            await _taskRepo.SaveChangesAsync();//önce kayıt yapıyoruz ki taskId oluşsun

            
        }
    }
}
