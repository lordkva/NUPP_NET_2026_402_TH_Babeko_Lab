using ComputerClub.Infrastructure.Models;
using ComputerClub.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
namespace ComputerClub.Infrastructure.Services;
public class CrudServiceAsync<T> : ICrudServiceAsync<T>
    where T : DbEntity
{
    private readonly IRepository<T> _repository;
    private readonly ComputerClubContext _context;

    public CrudServiceAsync(IRepository<T> repository, ComputerClubContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<bool> CreateAsync(T element)
    {
        try
        {
            if (element.DomainId == Guid.Empty) element.DomainId = Guid.NewGuid();
            await _repository.AddAsync(element);
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }
    public async Task<T> ReadAsync(Guid id)
    {
        var entities = await _repository.GetAllAsync();

        return entities.FirstOrDefault(e => e.DomainId == id)
            ?? throw new KeyNotFoundException($"Об'єкт з DomainId={id} не знайдено.");
    }
    public async Task<IEnumerable<T>> ReadAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
    {
        if (page < 1)
            throw new ArgumentOutOfRangeException(nameof(page));

        if (amount < 1)
            throw new ArgumentOutOfRangeException(nameof(amount));

        var entities = await _repository.GetAllAsync();

        return entities
            .OrderBy(e => e.Id)
            .Skip((page - 1) * amount)
            .Take(amount)
            .ToList();
    }
    public async Task<bool> UpdateAsync(T element)
    {
        try
        {
            await _repository.Update(element);
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }
    public async Task<bool> RemoveAsync(T element)
    {
        try
        {
            await _repository.Delete(element);
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }
    public Task<bool> SaveAsync()
    {
        return Task.FromResult(true);
    }
}