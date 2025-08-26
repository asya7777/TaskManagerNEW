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
    public class AddTagHandler
    {
        private readonly ITaskRepository _taskRepo;
        private readonly ITagRepository _tagRepo;
        public AddTagHandler(ITaskRepository taskRepo, ITagRepository tagRepo)
        {
            _taskRepo = taskRepo;
            _tagRepo = tagRepo;
        }

        public async System.Threading.Tasks.Task<Tag> HandleAsync(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                return null;
            }

            var existingTag = await _tagRepo.GetTagsByNameAsync(tagName);
            if (existingTag != null)
            {
                return existingTag; // Tag already exists, no need to add
            }

            var newTag = new Tag
            {
                tagName = tagName
            };

            await _tagRepo.AddAsync(newTag);
            await _tagRepo.SaveChangesAsync();
            return newTag;
        }

        public async System.Threading.Tasks.Task<List<string>> GetAllTagsAsync()
        {
            return await _tagRepo.GetAllTagsAsync();
        }
    }
}
