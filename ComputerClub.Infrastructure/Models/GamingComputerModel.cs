namespace ComputerClub.Infrastructure.Models;

public class GamingComputerModel : ComputerModel
{
    public string VideoCard { get; set; } = string.Empty;
    public int RefreshRate { get; set; }

    public GamingComputerDetailsModel? Details { get; set; }
}