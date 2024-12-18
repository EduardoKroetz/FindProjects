using FindProjects.Core.Entities;
using FindProjects.Core.Repositories.Base;

namespace FindProjects.Core.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> GetCategoriesByIds(List<int> ids);
    Task<Category?> GetByName(string name);
    Task<IEnumerable<Category>> GetAllAsync();
}