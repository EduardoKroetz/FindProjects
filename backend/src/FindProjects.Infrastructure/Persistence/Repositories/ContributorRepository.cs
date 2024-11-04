using FindProjects.Core.Entities;
using FindProjects.Core.Repositories;
using FindProjects.Infrastructure.Persistence.Repositories.Base;

namespace FindProjects.Infrastructure.Persistence.Repositories;

public class ContributorRepository : Repository<Contributor>, IContributorRepository
{
    public ContributorRepository(FindProjectsDbContext context) : base(context)
    {
    }
}