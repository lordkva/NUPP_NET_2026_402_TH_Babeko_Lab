using ComputerClub.Common;
CrudService<GamingComputer> service = new();
service.ItemCreated += computer =>
    Console.WriteLine($"Додано комп'ютер: {computer.Name}");

GamingComputer pc1 = new(
    "Gaming PC 1", "Intel Core i5", 16,
    "RTX 4060", 144, 80m);

GamingComputer pc2 = new(
    "Gaming PC 2", "AMD Ryzen 5", 32,
    "RTX 4070", 165, 100m);
service.Create(pc1);
service.Create(pc2);
Console.WriteLine("\nКомп'ютери клубу:");
foreach (GamingComputer pc in service.ReadAll())
{
    pc.ShowInfo();
}
Console.WriteLine("\nПошук комп'ютера:");
GamingComputer? found = service.Read(pc1.Id);
found?.ShowInfo();
pc1.PricePerHour = 90m;
service.Update(pc1);
Console.WriteLine("\nПісля оновлення:");
service.Read(pc1.Id)?.ShowInfo();
Console.WriteLine(
    $"\nВартість 3 годин: {pc1.CalculateCost(3)} грн");

Console.WriteLine(
    $"Усього створено комп'ютерів: {Computer.GetTotalComputers()}");

Player player = new("Андрій", 19, "Player01");
player.ShowInfo();

GamingSession session = new(
    player.Id, pc1.Id, DateTime.Now, 3);

session.ShowInfo();

string filePath = "computers.json";
service.Save(filePath);

Console.WriteLine("\nДані збережено у файл.");

service.Remove(pc2);

Console.WriteLine("\nПісля видалення:");

foreach (GamingComputer pc in service.ReadAll())
{
    pc.ShowInfo();
}

CrudService<GamingComputer> loadedService = new();
loadedService.Load(filePath);

Console.WriteLine("\nЗавантажені з файлу комп'ютери:");

foreach (GamingComputer pc in loadedService.ReadAll())
{
    pc.ShowInfo();
}

Console.WriteLine("\nПрограму завершено.");