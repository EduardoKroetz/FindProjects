using FindProjects.Core.Entities;
using FindProjects.Core.Repositories.Base;

namespace FindProjects.Core.Repositories;

public interface ISkillRepository : IRepository<Skill>
{
    Task<List<Skill>> GetSkillsByIds(List<int> ids);
}