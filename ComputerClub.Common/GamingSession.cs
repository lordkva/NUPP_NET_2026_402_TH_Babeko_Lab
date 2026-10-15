namespace ComputerClub.Common;
public class GamingSession : IEntity
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid ComputerId { get; set; }
    public DateTime StartTime { get; set; }
    public int Hours { get; set; }

    public GamingSession(
        Guid playerId,
        Guid computerId,
        DateTime startTime,
        int hours)
    {
        Id = Guid.NewGuid();
        PlayerId = playerId;
        ComputerId = computerId;
        StartTime = startTime;
        Hours = hours;
    }
    public void ShowInfo()
    {
        Console.WriteLine($"Сесія: {StartTime}, тривалість: {Hours} год.");
    }
}