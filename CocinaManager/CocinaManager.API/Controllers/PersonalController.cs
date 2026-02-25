using Microsoft.AspNetCore.Mvc;
using CocinaManager.Application.DTOs;
using CocinaManager.Application.Interfaces;

namespace CocinaManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonalController : ControllerBase
{
    private readonly IPersonalService _service;

    public PersonalController(IPersonalService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<PersonalDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PersonalDto>> GetById(Guid id)
    {
        var personal = await _service.GetByIdAsync(id);

        if (personal == null)
            return NotFound();

        return Ok(personal);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreatePersonalDto dto)
    {
        var id = await _service.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}