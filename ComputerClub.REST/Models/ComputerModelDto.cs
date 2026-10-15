using System.ComponentModel.DataAnnotations;

namespace ComputerClub.REST.Models;

public class ComputerModelDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Processor { get; set; } = string.Empty;
    public int RamGb { get; set; }
    public bool IsAvailable { get; set; }
    public string? VideoCard { get; set; }
    public int? RefreshRate { get; set; }
    public string? OfficeSoftware { get; set; }
}

public class CreateComputerDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Processor { get; set; } = string.Empty;

    [Range(1, 1024)]
    public int RamGb { get; set; }

    public bool IsAvailable { get; set; } = true;

    [Required]
    public string Type { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? VideoCard { get; set; }

    [Range(1, 1000)]
    public int? RefreshRate { get; set; }

    [MaxLength(200)]
    public string? OfficeSoftware { get; set; }
}

public class UpdateComputerDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Processor { get; set; } = string.Empty;

    [Range(1, 1024)]
    public int RamGb { get; set; }

    public bool IsAvailable { get; set; }

    [MaxLength(100)]
    public string? VideoCard { get; set; }

    [Range(1, 1000)]
    public int? RefreshRate { get; set; }

    [MaxLength(200)]
    public string? OfficeSoftware { get; set; }
}