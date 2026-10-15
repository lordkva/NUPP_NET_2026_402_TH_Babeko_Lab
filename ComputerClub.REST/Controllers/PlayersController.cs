using ComputerClub.Infrastructure.Models;
using ComputerClub.Infrastructure.Services;
using ComputerClub.REST.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComputerClub.REST.Controllers;

[ApiController]
[Route("api/players")]
public class PlayersController : ControllerBase
{
    private readonly ICrudServiceAsync<PlayerModel> _service;

    public PlayersController(ICrudServiceAsync<PlayerModel> service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int amount = 20)
    {
        if (page < 1 || amount < 1 || amount > 100)
            return BadRequest("page має бути >= 1, amount — від 1 до 100.");

        var players = await _service.ReadAllAsync(page, amount);
        return Ok(players.Select(ToDto));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PlayerDto>> GetById(Guid id)
    {
        try
        {
            return Ok(ToDto(await _service.ReadAsync(id)));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<PlayerDto>> Create(CreatePlayerDto dto)
    {
        var player = new PlayerModel
        {
            Name = dto.Name,
            Email = dto.Email
        };

        if (!await _service.CreateAsync(player))
            return Conflict("Не вдалося створити гравця. Перевір, чи Email не зайнятий.");

        return CreatedAtAction(
            nameof(GetById),
            new { id = player.DomainId },
            ToDto(player));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdatePlayerDto dto)
    {
        PlayerModel player;

        try
        {
            player = await _service.ReadAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        player.Name = dto.Name;
        player.Email = dto.Email;

        if (!await _service.UpdateAsync(player))
            return Conflict("Не вдалося оновити гравця. Перевір, чи Email не зайнятий.");

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        PlayerModel player;

        try
        {
            player = await _service.ReadAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        if (!await _service.RemoveAsync(player))
            return Conflict("Не вдалося видалити гравця.");

        return NoContent();
    }

    private static PlayerDto ToDto(PlayerModel player)
    {
        return new PlayerDto
        {
            Id = player.DomainId,
            Name = player.Name,
            Email = player.Email
        };
    }
}