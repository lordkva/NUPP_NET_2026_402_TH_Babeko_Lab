namespace ComputerClub.Common;
public interface ICrudService<T> where T : IEntity
{
    void Create(T element);
    T? Read(Guid id);
    IEnumerable<T> ReadAll();
    void Update(T element);
    void Remove(T element);

    void Save(string filePath);
    void Load(string filePath);
}