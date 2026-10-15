using ComputerClub.Infrastructure;
using ComputerClub.Infrastructure.Models;
using ComputerClub.Infrastructure.Repositories;
using ComputerClub.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
var databasePath = Path.Combine(
    Directory.GetCurrentDirectory(),
    "computerclub.db");
var options = new DbContextOptionsBuilder<ComputerClubContext>()
    .UseSqlite($"Data Source={databasePath}")
    .Options;
await using var context = new ComputerClubContext(options);
await context.Database.MigrateAsync();
var playerRepository = new Repository<PlayerModel>(context);
var playerService = new CrudServiceAsync<PlayerModel>(playerRepository, context);
var computerRepository = new Repository<GamingComputerModel>(context);
var computerService = new CrudServiceAsync<GamingComputerModel>(
    computerRepository, context);
Console.WriteLine("=== COMPUTER CLUB: LAB 3 ===");
var player = new PlayerModel
{
    Name = "Ivan",
    Email = $"ivan_{Guid.NewGuid():N}@example.com"
};
var playerCreated = await playerService.CreateAsync(player);
Console.WriteLine($"Гравця створено: {playerCreated}");
var computer = new GamingComputerModel
{
    Name = "Gaming PC",
    Processor = "Intel Core i5",
    RamGb = 16,
    VideoCard = "NVIDIA GeForce RTX 4060",
    RefreshRate = 144,
    Details = new GamingComputerDetailsModel
    {
        Keyboard = "Mechanical",
        Mouse = "Gaming Mouse",
        Monitor = "144 Hz"
    }
};
var computerCreated = await computerService.CreateAsync(computer);
Console.WriteLine($"Комп'ютер створено: {computerCreated}");
var session = new GamingSessionModel
{
    PlayerId = player.Id,
    ComputerId = computer.Id,
    StartTime = DateTime.Now,
    DurationMinutes = 60
};
await context.GamingSessions.AddAsync(session);
await context.SaveChangesAsync();
Console.WriteLine("Ігровий сеанс створено.");
var players = await playerService.ReadAllAsync();
Console.WriteLine("\nСписок гравців:");
foreach (var item in players)
{
    Console.WriteLine(
        $"Id: {item.Id}, Ім'я: {item.Name}, Email: {item.Email}");
}
var firstPage = await playerService.ReadAllAsync(1, 5);
Console.WriteLine($"\nГравців на першій сторінці: {firstPage.Count()}");
var sessions = await context.GamingSessions.Include(s => s.Player).Include(s => s.Computer).ToListAsync();
Console.WriteLine("\nІгрові сеанси:");
foreach (var item in sessions)
{
    Console.WriteLine($"{item.Player.Name} — {item.Computer.Name}, " + $"{item.DurationMinutes} хв.");
}
Console.WriteLine($"\nФайл БД: {databasePath}");
Console.WriteLine("Роботу завершено.");