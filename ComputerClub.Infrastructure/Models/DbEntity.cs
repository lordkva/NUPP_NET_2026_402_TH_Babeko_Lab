namespace ComputerClub.Infrastructure.Models;

public abstract class DbEntity
{
    public int Id { get; set; }

    public Guid DomainId { get; set; } = Guid.NewGuid();
}