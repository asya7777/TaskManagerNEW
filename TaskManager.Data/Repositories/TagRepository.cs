using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Data;
using TaskManager.Entities;

namespace TaskManager.Infrastructure.Repositories
{
    public class TagRepository:ITagRepository
    {
        private readonly AppDbContext _context;

        public TagRepository(AppDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task AddAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
        }

        public async System.Threading.Tasks.Task<Tag?> GetTagsByNameAsync(string name)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.tagName == name);
        }

        public async System.Threading.Tasks.Task<List<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async System.Threading.Tasks.Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
