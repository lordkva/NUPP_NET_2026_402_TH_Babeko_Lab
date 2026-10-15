using ComputerClub.Infrastructure;
using ComputerClub.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerClub.MVC.Controllers;

public class ComputersController : Controller
{
    private readonly ComputerClubContext _context;

    public ComputersController(ComputerClubContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var computers = await _context.Computers
            .AsNoTracking()
            .ToListAsync();

        return View(computers);
    }
}