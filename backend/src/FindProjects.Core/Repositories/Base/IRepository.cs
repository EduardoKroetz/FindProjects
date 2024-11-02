namespace FindProjects.Core.Repositories.Base;

public interface IRepository<T>
{
    Task<T?> GetById(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task RemoveAsync(T entity);
}