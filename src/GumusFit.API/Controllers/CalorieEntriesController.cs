using GumusFit.Application.DTOs;
using GumusFit.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GumusFit.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalorieEntriesController : ControllerBase
{
    private readonly ICalorieService _calorieService;

    public CalorieEntriesController(ICalorieService calorieService)
    {
        _calorieService = calorieService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var entries = await _calorieService.GetAllAsync();
        return Ok(entries);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var entries = await _calorieService.GetByUserIdAsync(userId);
        return Ok(entries);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var entry = await _calorieService.GetByIdAsync(id);
        if (entry is null) return NotFound();
        return Ok(entry);
    }

    [HttpPost("user/{userId:guid}")]
    public async Task<IActionResult> Create(Guid userId, [FromBody] CreateCalorieEntryDto dto)
    {
        var created = await _calorieService.CreateAsync(userId, dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateCalorieEntryDto dto)
    {
        var updated = await _calorieService.UpdateAsync(id, dto);
        if (!updated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _calorieService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
