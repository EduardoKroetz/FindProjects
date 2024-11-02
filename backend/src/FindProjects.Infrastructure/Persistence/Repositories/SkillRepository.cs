using FindProjects.Core.Entities;
using FindProjects.Core.Repositories;
using FindProjects.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace FindProjects.Infrastructure.Persistence.Repositories;

public class SkillRepository : Repository<Skill>, ISkillRepository
{
    public SkillRepository(FindProjectsDbContext context) : base(context) {}
    
    public async Task<List<Skill>> GetSkillsByIds(List<int> ids)
    {
        return await _context.Skills
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
    }
}