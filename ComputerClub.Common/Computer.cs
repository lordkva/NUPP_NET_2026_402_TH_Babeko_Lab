namespace ComputerClub.Common;
public abstract class Computer : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Processor { get; set; }
    public int Ram { get; set; }
    public static int TotalComputers;
    static Computer()
    {
        TotalComputers = 0;
    }
    protected Computer(string name, string processor, int ram)
    {
        Id = Guid.NewGuid();
        Name = name;
        Processor = processor;
        Ram = ram;
        Interlocked.Increment(ref TotalComputers);
    }
    public virtual void ShowInfo()
    {
        Console.WriteLine(
            $"{Name}, процесор: {Processor}, RAM: {Ram} GB");
    }
    public static int GetTotalComputers()
    {
        return Volatile.Read(ref TotalComputers);
    }
}