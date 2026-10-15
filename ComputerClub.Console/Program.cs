using System.Collections.Concurrent;
using ComputerClub.Common;

const int count = 2000;
CrudServiceAsync<GamingComputer> service = new("computers.json");
ConcurrentBag<GamingComputer> computers = new();
Parallel.For(0, count, _ =>
{
    computers.Add(GamingComputer.CreateNew());
});
Console.WriteLine($"Створено об'єктів: {computers.Count}");
Task<bool>[] tasks = computers.Select(computer => service.CreateAsync(computer)).ToArray();
bool[] results = await Task.WhenAll(tasks);
Console.WriteLine($"Додано до сервісу: {results.Count(x => x)}");
List<GamingComputer> allComputers = (await service.ReadAllAsync()).ToList();
Console.WriteLine("\nСтатистика:");
Console.WriteLine($"Мінімальна ціна: {allComputers.Min(x => x.PricePerHour)} грн");
Console.WriteLine($"Максимальна ціна: {allComputers.Max(x => x.PricePerHour)} грн");
Console.WriteLine($"Середня ціна: {allComputers.Average(x => x.PricePerHour):F2} грн");
Console.WriteLine($"Мінімальна RAM: {allComputers.Min(x => x.Ram)} GB");
Console.WriteLine($"Максимальна RAM: {allComputers.Max(x => x.Ram)} GB");
Console.WriteLine($"Середня RAM: {allComputers.Average(x => x.Ram):F2} GB");
Console.WriteLine("\nПерша сторінка:");
IEnumerable<GamingComputer> page = await service.ReadAllAsync(1, 10);
foreach (GamingComputer computer in page)
{
    computer.ShowInfo();
}
Console.WriteLine("\nПеребір через IEnumerable:");
int shown = 0;
foreach (GamingComputer computer in service)
{
    computer.ShowInfo();
    shown++;
    if (shown == 3)
        break;
}

bool saved = await service.SaveAsync();

Console.WriteLine(saved ? "\nДані збережено." : "\nПомилка збереження.");

Console.WriteLine("\nПриклад lock:");
SynchronizationExamples.LockExample();

Console.WriteLine("\nПриклад SemaphoreSlim:");
await SynchronizationExamples.SemaphoreExample();

Console.WriteLine("\nПриклад AutoResetEvent:");
SynchronizationExamples.AutoResetEventExample();

Console.WriteLine("\nПрограму завершено.");