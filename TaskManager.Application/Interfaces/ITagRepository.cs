using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface ITagRepository
    {
        System.Threading.Tasks.Task AddAsync(Tag tag); // change in memory
        System.Threading.Tasks.Task<Tag?> GetTagsByNameAsync(string name);
        System.Threading.Tasks.Task<List<Tag>> GetAllTagsAsync();
        System.Threading.Tasks.Task SaveChangesAsync(); // commit changes to the database
    }
}
