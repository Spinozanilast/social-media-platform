using Microsoft.EntityFrameworkCore;
using StoriesService.Common;
using StoriesService.Data;
using StoriesService.Entities;

namespace StoriesService.Repositories;

public class StoryRepository(StoriesDbContext context) : IStoryRepository
{
    private readonly StoriesDbContext _context = context;

    public async Task<IEnumerable<Story>> GetAllStoriesAsync(string? tag = null, Guid? authorId = null,
        int pageNumber = 1, int pageSize = 10)
    {
        var query = _context.Stories.AsQueryable();
        if (!string.IsNullOrWhiteSpace(tag))
        {
            query = query.Where(s => s.Tags.Contains(tag));
        }

        if (authorId.HasValue)
        {
            query = query.Where(s => s.AuthorId == authorId);
        }

        return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public Task<int> GetAllStoriesAsync(string? tag, Guid? authorId)
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetAllStoriesCountAsync(string? tag = null, Guid? authorId = null)
    {
        var query = _context.Stories.AsQueryable();
        if (!string.IsNullOrWhiteSpace(tag))
        {
            query = query.Where(s => s.Tags.Contains(tag));
        }

        if (authorId.HasValue)
        {
            query = query.Where(s => s.AuthorId == authorId);
        }

        return await query.CountAsync();
    }

    public async Task<Story?> GetStoryByIdAsync(int id)
    {
        return await _context.Stories.FindAsync(id);
    }

    public async Task AddStoryAsync(Story story)
    {
        await _context.Stories.AddAsync(story);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStoryAsync(Story story)
    {
        _context.Stories.Update(story);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteStoryAsync(int id)
    {
        var story = await _context.Stories.FindAsync(id);
        if (story != null)
        {
            _context.Stories.Remove(story);
            await _context.SaveChangesAsync();
        }
    }
}