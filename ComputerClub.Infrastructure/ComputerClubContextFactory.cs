using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ComputerClub.Infrastructure;

public class ComputerClubContextFactory
    : IDesignTimeDbContextFactory<ComputerClubContext>
{
    public ComputerClubContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ComputerClubContext>();

        optionsBuilder.UseSqlite("Data Source=computerclub.db");

        return new ComputerClubContext(optionsBuilder.Options);
    }
}