namespace ComputerClub.Infrastructure.Models;

public class GamingComputerDetailsModel : DbEntity
{
    public int GamingComputerId { get; set; }

    public string Keyboard { get; set; } = string.Empty;
    public string Mouse { get; set; } = string.Empty;
    public string Monitor { get; set; } = string.Empty;

    public GamingComputerModel GamingComputer { get; set; } = null!;
}