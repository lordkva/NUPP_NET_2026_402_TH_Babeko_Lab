
using ComputerClub.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ComputerClub.REST.Controllers;

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "Admin")]
public class AdminUsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminUsersController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole(
        [FromBody] AssignRoleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new { message = "Email обов'язковий." });
        }

        if (request.Role != "Employee" && request.Role != "Client")
        {
            return BadRequest(new
            {
                message = "Можна призначити лише роль Employee або Client."
            });
        }

        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return NotFound(new { message = "Користувача не знайдено." });
        }

        if (!await _roleManager.RoleExistsAsync(request.Role))
        {
            return BadRequest(new { message = "Роль не існує." });
        }

        if (await _userManager.IsInRoleAsync(user, request.Role))
        {
            return Ok(new
            {
                message = "Користувач уже має цю роль.",
                user.Email,
                request.Role
            });
        }

        var result = await _userManager.AddToRoleAsync(user, request.Role);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                errors = result.Errors.Select(e => e.Description)
            });
        }

        return Ok(new
        {
            message = "Роль успішно призначено.",
            user.Email,
            request.Role
        });
    }
}

public record AssignRoleRequest(string Email, string Role);
