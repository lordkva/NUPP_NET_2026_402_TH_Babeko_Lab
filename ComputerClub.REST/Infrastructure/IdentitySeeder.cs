
using ComputerClub.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace ComputerClub.REST.Infrastructure;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(IServiceProvider services)
    {
        var roleManager =
            services.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = { "Admin", "Employee", "Client" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(
                    new IdentityRole(role));

                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        result.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Не вдалося створити роль {role}: {errors}");
                }
            }
        }
    }

    public static async Task SeedAdminAsync(
        IServiceProvider services,
        IConfiguration configuration)
    {
        var email = configuration["AdminSeed:Email"];
        var password = configuration["AdminSeed:Password"];

        // Не створюємо адміністратора, якщо секрети не налаштовані.
        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        var userManager =
            services.GetRequiredService<UserManager<ApplicationUser>>();

        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var createResult =
                await userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    createResult.Errors.Select(e => e.Description));

                throw new InvalidOperationException(
                    $"Не вдалося створити Admin: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(user, "Admin"))
        {
            var roleResult =
                await userManager.AddToRoleAsync(user, "Admin");

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    roleResult.Errors.Select(e => e.Description));

                throw new InvalidOperationException(
                    $"Не вдалося призначити роль Admin: {errors}");
            }
        }
    }
}
