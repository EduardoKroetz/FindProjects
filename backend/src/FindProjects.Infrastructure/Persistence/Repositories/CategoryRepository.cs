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
}