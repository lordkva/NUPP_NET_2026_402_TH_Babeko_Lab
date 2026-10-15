namespace ComputerClub.Infrastructure.Models;

public class PlayerModel : DbEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public ICollection<GamingSessionModel> Sessions { get; set; }
        = new List<GamingSessionModel>();
}