using Microsoft.AspNetCore.Identity;
namespace ComputerClub.Infrastructure.Models;
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
}