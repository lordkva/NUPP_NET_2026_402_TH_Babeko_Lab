namespace ComputerClub.Infrastructure.Models;

public abstract class ComputerModel : DbEntity
{
    public string Name { get; set; } = string.Empty;
    public string Processor { get; set; } = string.Empty;
    public int RamGb { get; set; }
    public bool IsAvailable { get; set; } = true;

    public ICollection<GamingSessionModel> Sessions { get; set; }
        = new List<GamingSessionModel>();
}