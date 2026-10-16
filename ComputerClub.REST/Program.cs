using ComputerClub.Infrastructure;
using ComputerClub.Infrastructure.Models;
using ComputerClub.Infrastructure.Repositories;
using ComputerClub.Infrastructure.Services;
using ComputerClub.REST.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ComputerClubContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("ComputerClub")
        ?? "Data Source=computerclub.db"));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(ICrudServiceAsync<>), typeof(CrudServiceAsync<>));

builder.Services
    .AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ComputerClubContext>();


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Встав сюди accessToken, отриманий після входу."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ComputerClubContext>();
    await context.Database.MigrateAsync();

    await IdentitySeeder.SeedRolesAsync(services);
    await IdentitySeeder.SeedAdminAsync(services, builder.Configuration);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapPost("/auth/register", async (
    CustomRegisterRequest request,
    UserManager<ApplicationUser> userManager) =>
{
    if (string.IsNullOrWhiteSpace(request.Email) ||
        string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.BadRequest(new
        {
            message = "Email і пароль обов'язкові."
        });
    }

    var user = new ApplicationUser
    {
        UserName = request.Email,
        Email = request.Email,
        FirstName = request.FirstName ?? string.Empty,
        LastName = request.LastName ?? string.Empty
    };

    var result = await userManager.CreateAsync(
        user, request.Password);

    if (!result.Succeeded)
    {
        return Results.ValidationProblem(
            result.Errors.ToDictionary(
                error => error.Code,
                error => new[] { error.Description }));
    }

    var roleResult = await userManager.AddToRoleAsync(
        user, "Client");

    if (!roleResult.Succeeded)
    {
        await userManager.DeleteAsync(user);

        return Results.Problem(
            "Не вдалося призначити роль користувачу.");
    }

    return Results.Created(
        $"/auth/users/{user.Id}",
        new
        {
            user.Id,
            user.Email,
            Role = "Client"
        });
});

app.MapGroup("/identity")
    .MapIdentityApi<ApplicationUser>();

app.Run();

public record CustomRegisterRequest(
    string Email,
    string Password,
    string? FirstName,
    string? LastName);