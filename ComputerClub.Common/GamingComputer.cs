namespace ComputerClub.Common;
public class GamingComputer : Computer
{
    public string VideoCard { get; set; }
    public int MonitorHz { get; set; }
    public decimal PricePerHour { get; set; }
    public GamingComputer(
        string name,
        string processor,
        int ram,
        string videoCard,
        int monitorHz,
        decimal pricePerHour)
        : base(name, processor, ram)
    {
        VideoCard = videoCard;
        MonitorHz = monitorHz;
        PricePerHour = pricePerHour;
    }
    public override void ShowInfo()
    {
        Console.WriteLine(
            $"Ігровий ПК: {Name}, CPU: {Processor}, RAM: {Ram} GB, " +
            $"GPU: {VideoCard}, монітор: {MonitorHz} Hz, " +
            $"ціна: {PricePerHour} грн/год");
    }
}