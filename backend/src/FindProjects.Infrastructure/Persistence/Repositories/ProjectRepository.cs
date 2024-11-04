using FindProjects.Core.Entities;
using FindProjects.Core.Repositories;
using FindProjects.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace FindProjects.Infrastructure.Persistence.Repositories;

public class ProjectRepository : Repository<Project>, IProjectRepository
{
    public ProjectRepository(FindProjectsDbContext context) : base(context) {}

    public async Task<Project?> GetProjectWithRelations(int projectId)
    {
        return await _context.Projects
            .Include(x => x.Categories)
            .Include(x => x.Skills)
            .Include(x => x.Contributors)
            .FirstOrDefaultAsync(x => x.Id == projectId);
    }
}