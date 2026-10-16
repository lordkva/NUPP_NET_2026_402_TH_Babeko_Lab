using ComputerClub.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace ComputerClub.Infrastructure;

public class ComputerClubContext: IdentityDbContext<ApplicationUser>
{
    public ComputerClubContext(DbContextOptions<ComputerClubContext> options)
        : base(options)
    {
    }

    public DbSet<ComputerModel> Computers => Set<ComputerModel>();
    public DbSet<GamingComputerModel> GamingComputers => Set<GamingComputerModel>();
    public DbSet<OfficeComputerModel> OfficeComputers => Set<OfficeComputerModel>();
    public DbSet<PlayerModel> Players => Set<PlayerModel>();
    public DbSet<GamingSessionModel> GamingSessions => Set<GamingSessionModel>();
    public DbSet<GamingComputerDetailsModel> GamingComputerDetails
        => Set<GamingComputerDetailsModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ComputerModel>().UseTptMappingStrategy().ToTable("Computers");
        modelBuilder.Entity<GamingComputerModel>().ToTable("GamingComputers");
        modelBuilder.Entity<OfficeComputerModel>().ToTable("OfficeComputers");
        modelBuilder.Entity<ComputerModel>().HasKey(c => c.Id);
        modelBuilder.Entity<ComputerModel>().Property(c => c.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<ComputerModel>().Property(c => c.Processor).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<PlayerModel>().ToTable("Players");
        modelBuilder.Entity<PlayerModel>().HasKey(p => p.Id);
        modelBuilder.Entity<PlayerModel>().Property(p => p.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<PlayerModel>().Property(p => p.Email).HasMaxLength(150).IsRequired();
        modelBuilder.Entity<PlayerModel>().HasIndex(p => p.Email).IsUnique();
        modelBuilder.Entity<GamingSessionModel>().ToTable("GamingSessions");
        modelBuilder.Entity<GamingSessionModel>().HasKey(s => s.Id);
        modelBuilder.Entity<GamingSessionModel>().HasOne(s => s.Player).WithMany(p => p.Sessions).HasForeignKey(s => s.PlayerId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<GamingSessionModel>().HasOne(s => s.Computer).WithMany(c => c.Sessions).HasForeignKey(s => s.ComputerId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<GamingComputerDetailsModel>().ToTable("GamingComputerDetails");
        modelBuilder.Entity<GamingComputerDetailsModel>().HasKey(d => d.Id);
        modelBuilder.Entity<GamingComputerDetailsModel>().HasOne(d => d.GamingComputer).WithOne(c => c.Details).HasForeignKey<GamingComputerDetailsModel>(d => d.GamingComputerId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<GamingComputerDetailsModel>().HasIndex(d => d.GamingComputerId).IsUnique();
        modelBuilder.Entity<GamingComputerModel>().Property(c => c.VideoCard).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<OfficeComputerModel>().Property(c => c.OfficeSoftware).HasMaxLength(200).IsRequired();
        modelBuilder.Entity<GamingSessionModel>().Property(s => s.DurationMinutes).IsRequired();
    }
}