using ComputerClub.Infrastructure.Models;
using ComputerClub.Infrastructure.Services;
using ComputerClub.REST.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ComputerClub.REST.Controllers;

[ApiController]
[Route("api/computers")]
public class ComputersController : ControllerBase
{
    private readonly ICrudServiceAsync<ComputerModel> _service;

    public ComputersController(ICrudServiceAsync<ComputerModel> service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ComputerModelDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int amount = 20)
    {
        if (page < 1 || amount < 1 || amount > 100)
            return BadRequest("page має бути >= 1, amount — від 1 до 100.");

        var computers = await _service.ReadAllAsync(page, amount);
        return Ok(computers.Select(ToDto));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ComputerModelDto>> GetById(Guid id)
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
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<ComputerModelDto>> Create(
        CreateComputerDto dto)
    {
        ComputerModel computer;

        switch (dto.Type.Trim().ToLowerInvariant())
        {
            case "gaming":
                if (string.IsNullOrWhiteSpace(dto.VideoCard) ||
                    dto.RefreshRate is null)
                {
                    return BadRequest(
                        "Для ігрового комп'ютера потрібні VideoCard і RefreshRate.");
                }

                computer = new GamingComputerModel
                {
                    Name = dto.Name,
                    Processor = dto.Processor,
                    RamGb = dto.RamGb,
                    IsAvailable = dto.IsAvailable,
                    VideoCard = dto.VideoCard,
                    RefreshRate = dto.RefreshRate.Value
                };
                break;

            case "office":
                if (string.IsNullOrWhiteSpace(dto.OfficeSoftware))
                {
                    return BadRequest(
                        "Для офісного комп'ютера потрібне OfficeSoftware.");
                }

                computer = new OfficeComputerModel
                {
                    Name = dto.Name,
                    Processor = dto.Processor,
                    RamGb = dto.RamGb,
                    IsAvailable = dto.IsAvailable,
                    OfficeSoftware = dto.OfficeSoftware
                };
                break;

            default:
                return BadRequest("Type повинен бути gaming або office.");
        }

        var created = await _service.CreateAsync(computer);

        if (!created)
            return Conflict("Не вдалося створити комп'ютер.");

        return CreatedAtAction(
            nameof(GetById),
            new { id = computer.DomainId },
            ToDto(computer));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> Update(Guid id, UpdateComputerDto dto)
    {
        ComputerModel computer;

        try
        {
            computer = await _service.ReadAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        computer.Name = dto.Name;
        computer.Processor = dto.Processor;
        computer.RamGb = dto.RamGb;
        computer.IsAvailable = dto.IsAvailable;

        if (computer is GamingComputerModel gaming)
        {
            if (string.IsNullOrWhiteSpace(dto.VideoCard) ||
                dto.RefreshRate is null)
            {
                return BadRequest(
                    "Для ігрового комп'ютера потрібні VideoCard і RefreshRate.");
            }

            gaming.VideoCard = dto.VideoCard;
            gaming.RefreshRate = dto.RefreshRate.Value;
        }
        else if (computer is OfficeComputerModel office)
        {
            if (string.IsNullOrWhiteSpace(dto.OfficeSoftware))
            {
                return BadRequest(
                    "Для офісного комп'ютера потрібне OfficeSoftware.");
            }

            office.OfficeSoftware = dto.OfficeSoftware;
        }

        if (!await _service.UpdateAsync(computer))
            return Conflict("Не вдалося оновити комп'ютер.");

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        ComputerModel computer;

        try
        {
            computer = await _service.ReadAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        if (!await _service.RemoveAsync(computer))
            return Conflict("Не вдалося видалити комп'ютер.");

        return NoContent();
    }

    private static ComputerModelDto ToDto(ComputerModel computer)
    {
        var dto = new ComputerModelDto
        {
            Id = computer.DomainId,
            Name = computer.Name,
            Processor = computer.Processor,
            RamGb = computer.RamGb,
            IsAvailable = computer.IsAvailable
        };

        if (computer is GamingComputerModel gaming)
        {
            dto.Type = "gaming";
            dto.VideoCard = gaming.VideoCard;
            dto.RefreshRate = gaming.RefreshRate;
        }
        else if (computer is OfficeComputerModel office)
        {
            dto.Type = "office";
            dto.OfficeSoftware = office.OfficeSoftware;
        }

        return dto;
    }
}