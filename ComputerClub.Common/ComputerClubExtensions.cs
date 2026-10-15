namespace ComputerClub.Common;
public static class ComputerClubExtensions
{
    public static decimal CalculateCost(this GamingComputer computer,int hours)
    {
        return computer.PricePerHour * hours;
    }
}