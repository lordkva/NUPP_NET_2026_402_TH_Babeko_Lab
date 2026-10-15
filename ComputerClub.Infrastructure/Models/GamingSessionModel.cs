namespace ComputerClub.Infrastructure.Models;

public class GamingSessionModel : DbEntity
{
    public DateTime StartTime { get; set; } = DateTime.Now;
    public int DurationMinutes { get; set; }

    public int PlayerId { get; set; }
    public PlayerModel Player { get; set; } = null!;

    public int ComputerId { get; set; }
    public ComputerModel Computer { get; set; } = null!;
}