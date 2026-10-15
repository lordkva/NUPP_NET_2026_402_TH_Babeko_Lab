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
        Console.WriteLine($"{Name}, {Processor}, RAM: {Ram} GB, " + $"GPU: {VideoCard}, {MonitorHz} Hz, " + $"{PricePerHour} грн/год");
    }
    public static GamingComputer CreateNew()
    {
        string[] processors =
        {
            "Intel Core i5",
            "Intel Core i7",
            "AMD Ryzen 5",
            "AMD Ryzen 7"
        };

        string[] videoCards =
        {
            "RTX 4060",
            "RTX 4070",
            "RTX 4080"
        };

        return new GamingComputer(
            $"Gaming PC {Guid.NewGuid().ToString()[..8]}",
            processors[Random.Shared.Next(processors.Length)],
            Random.Shared.Next(16, 65) / 16 * 16,
            videoCards[Random.Shared.Next(videoCards.Length)],
            Random.Shared.Next(1, 4) * 60,
            Random.Shared.Next(50, 151));
    }
}