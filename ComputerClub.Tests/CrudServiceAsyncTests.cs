using ComputerClub.Common;
using Xunit;
namespace ComputerClub.Tests;

public class CrudServiceAsyncTests
{
    private static GamingComputer CreateComputer()
    {
        return new GamingComputer("Test PC", "Intel Core i5", 16, "RTX 4060", 144, 80m);
    }

    [Fact]
    public async Task CreateAndRead_ShouldWork()
    {
        string path = Path.GetTempFileName();
        var service = new CrudServiceAsync<GamingComputer>(path);

        try
        {
            GamingComputer computer = CreateComputer();

            Assert.True(await service.CreateAsync(computer));

            GamingComputer result =
                await service.ReadAsync(computer.Id);

            Assert.Equal(computer.Name, result.Name);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public async Task Update_ShouldChangePrice()
    {
        string path = Path.GetTempFileName();
        var service = new CrudServiceAsync<GamingComputer>(path);

        try
        {
            GamingComputer computer = CreateComputer();

            await service.CreateAsync(computer);
            computer.PricePerHour = 120m;

            Assert.True(await service.UpdateAsync(computer));

            GamingComputer result =
                await service.ReadAsync(computer.Id);

            Assert.Equal(120m, result.PricePerHour);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public async Task Remove_ShouldDeleteComputer()
    {
        string path = Path.GetTempFileName();
        var service = new CrudServiceAsync<GamingComputer>(path);

        try
        {
            GamingComputer computer = CreateComputer();

            await service.CreateAsync(computer);

            Assert.True(await service.RemoveAsync(computer));

            Assert.Empty(await service.ReadAllAsync());
        }
        finally
        {
            File.Delete(path);
        }
    }
}