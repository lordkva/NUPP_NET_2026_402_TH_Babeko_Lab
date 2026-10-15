namespace ComputerClub.Common;
public class OfficeComputer : Computer
{
    public string OperatingSystem { get; set; }
    public bool HasWebcam { get; set; }
    public decimal PricePerHour { get; set; }
    public OfficeComputer(
        string name,
        string processor,
        int ram,
        string operatingSystem,
        bool hasWebcam,
        decimal pricePerHour)
        : base(name, processor, ram)
    {
        OperatingSystem = operatingSystem;
        HasWebcam = hasWebcam;
        PricePerHour = pricePerHour;
    }
    public override void ShowInfo()
    {
        Console.WriteLine($"Офісний ПК: {Name}, CPU: {Processor}, RAM: {Ram} GB, " + $"ОС: {OperatingSystem}, вебкамера: {HasWebcam}, " + $"ціна: {PricePerHour} грн/год");
    }
}