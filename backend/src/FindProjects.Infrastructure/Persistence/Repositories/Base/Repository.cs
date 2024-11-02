using FindProjects.Core.Entities.Base;
using FindProjects.Core.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace FindProjects.Infrastructure.Persistence.Repositories.Base;

public class Repository<T> : IRepository<T> where T : Entity
{
    protected FindProjectsDbContext _context;

    public Repository(FindProjectsDbContext context)
    {
        _context = context;
    }

    public async Task<T?> GetById(int id)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}