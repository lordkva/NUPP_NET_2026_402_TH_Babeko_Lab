using System.Collections;
using System.Collections.Concurrent;
using System.Text.Json;

namespace ComputerClub.Common;
public class CrudServiceAsync<T> : ICrudServiceAsync<T>
    where T : IEntity
{
    private readonly ConcurrentDictionary<Guid, T> items = new();
    private readonly SemaphoreSlim semaphore = new(1, 1);
    private readonly string filePath;
    public CrudServiceAsync(string filePath)
    {
        this.filePath = filePath;
    }
    public async Task<bool> CreateAsync(T element)
    {
        await semaphore.WaitAsync();

        try
        {
            return items.TryAdd(element.Id, element);
        }
        finally
        {
            semaphore.Release();
        }
    }
    public async Task<T> ReadAsync(Guid id)
    {
        await semaphore.WaitAsync();
        try
        {
            if (items.TryGetValue(id, out T? element))
                return element;
            throw new KeyNotFoundException("Об'єкт не знайдено.");
        }
        finally
        {
            semaphore.Release();
        }
    }
    public async Task<IEnumerable<T>> ReadAllAsync()
    {
        await semaphore.WaitAsync();
        try
        {
            return items.Values.ToList();
        }
        finally
        {
            semaphore.Release();
        }
    }
    public async Task<IEnumerable<T>> ReadAllAsync(
        int page, int amount)
    {
        if (page < 1 || amount < 1)
            throw new ArgumentOutOfRangeException();
        await semaphore.WaitAsync();
        try
        {
            return items.Values
                .OrderBy(x => x.Id)
                .Skip((page - 1) * amount)
                .Take(amount)
                .ToList();
        }
        finally
        {
            semaphore.Release();
        }
    }
    public async Task<bool> UpdateAsync(T element)
    {
        await semaphore.WaitAsync();
        try
        {
            if (!items.ContainsKey(element.Id))
                return false;
            items[element.Id] = element;
            return true;
        }
        finally
        {
            semaphore.Release();
        }
    }
    public async Task<bool> RemoveAsync(T element)
    {
        await semaphore.WaitAsync();
        try
        {
            return items.TryRemove(element.Id, out _);
        }
        finally
        {
            semaphore.Release();
        }
    }
    public async Task<bool> SaveAsync()
    {
        await semaphore.WaitAsync();
        try
        {
            string json = JsonSerializer.Serialize(items.Values.ToList(), new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            await File.WriteAllTextAsync(filePath, json);
            return true;
        }
        catch (IOException)
        {
            return false;
        }
        finally
        {
            semaphore.Release();
        }
    }
    public IEnumerator<T> GetEnumerator()
    {
        return items.Values.ToList().GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}