
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
            _tagRepo= tagRepo;
        }

        public async System.Threading.Tasks.Task HandleAsync(CreateTaskDTO dto)
        {
            var newTask = new Domain.Entities.Task
            {
                taskName = dto.taskName,
                taskDescription = dto.taskDescription,
                taskDeadline = dto.taskDeadline,
                usrId = dto.usrId
            };

            await _taskRepo.AddAsync(newTask);
            await _taskRepo.SaveChangesAsync();//önce kayıt yapıyoruz ki taskId oluşsun

            if(dto.Tags != null && dto.Tags.Count > 0)//check if a request was sent
            {
                var normalized = dto.Tags
                    .Select(t => t.Trim())
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                var existingTags = await _tagRepo.GetAllTagsAsync();
                var dict = existingTags.ToDictionary(t=> t.tagName, StringComparer.OrdinalIgnoreCase);//removing duplicates

                foreach(var tagname in normalized)
                {
                    Tag tag;
                    if(!dict.TryGetValue(tagname, out tag))//check if the tag exists
                    {
                        tag= new Tag
                        {
                            tagName = tagname
                        };
                        await _tagRepo.AddAsync(tag);
                        dict[tagname] = tag; //add the new tag to the dictionary for later use!
                    }

                    newTask.Tags.Add(tag);//add the tag to the task
                }

                await _taskRepo.SaveChangesAsync();
            }
        }
    }
}
