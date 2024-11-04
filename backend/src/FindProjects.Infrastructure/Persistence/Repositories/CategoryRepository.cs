using FindProjects.Core.Entities;
using FindProjects.Core.Repositories;
using FindProjects.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace FindProjects.Infrastructure.Persistence.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(FindProjectsDbContext context) : base(context) {}
    
    public async Task<List<Category>> GetCategoriesByIds(List<int> ids)
    {
        return await _context.Categories
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
    }

    public async Task<Category?> GetByName(string name)
    {
        return await _context.Categories.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }
}