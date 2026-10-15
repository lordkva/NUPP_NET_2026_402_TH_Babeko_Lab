using System.Text.Json;
namespace ComputerClub.Common;

public delegate void ItemCreatedHandler<T>(T item);
public class CrudService<T> : ICrudService<T> where T : IEntity
{
    private List<T> items = new();
    public event ItemCreatedHandler<T>? ItemCreated;
    public void Create(T element)
    {
        items.Add(element);
        ItemCreated?.Invoke(element);
    }
    public T? Read(Guid id)
    {
        return items.FirstOrDefault(x => x.Id == id);
    }

    public IEnumerable<T> ReadAll()
    {
        return items;
    }

    public void Update(T element)
    {
        int index = items.FindIndex(x => x.Id == element.Id);
        if (index != -1)
        {
            items[index] = element;
        }
    }

    public void Remove(T element)
    {
        items.RemoveAll(x => x.Id == element.Id);
    }

    public void Save(string filePath)
    {
        string json = JsonSerializer.Serialize(
            items,
            new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public void Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return;
        }
        string json = File.ReadAllText(filePath);
        items = JsonSerializer.Deserialize<List<T>>(json)
                ?? new List<T>();
    }
}