using ComputerClub.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
namespace ComputerClub.Infrastructure.Repositories;
public class Repository<T> : IRepository<T> where T : DbEntity
{
    private readonly ComputerClubContext _context;
    private readonly DbSet<T> _dbSet;
    public Repository(ComputerClubContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    public async Task<T> GetByIdAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        return entity ?? throw new KeyNotFoundException($"Об'єкт типу {typeof(T).Name} з Id={id} не знайдено.");
    }
        public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}