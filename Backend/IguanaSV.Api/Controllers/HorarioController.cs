using IguanaSV.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IguanaSV.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HorarioController : ControllerBase
{
    private readonly IguanaContext _context;

    public HorarioController(IguanaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Horario>>> GetHorarios()
    {
        return await _context.Horarios
            .Include(h => h.Publicacion)
            .Include(h => h.ReservaHorarios)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Horario>> GetHorario(int id)
    {
        var horario = await _context.Horarios
            .Include(h => h.Publicacion)
            .Include(h => h.ReservaHorarios)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (horario == null)
        {
            return NotFound();
        }

        return horario;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutHorario(int id, Horario horario)
    {
        if (id != horario.Id)
        {
            return BadRequest();
        }

        var exists = await _context.Horarios.AnyAsync(h => h.Id == id);

        if (!exists)
        {
            return NotFound();
        }

        if (!await _context.Publicaciones.AnyAsync(p => p.Id == horario.PublicacionId))
        {
            return NotFound($"La publicacion con id {horario.PublicacionId} no existe.");
        }

        if (horario.HoraFin <= horario.HoraInicio)
        {
            return BadRequest("La hora de fin debe ser posterior a la hora de inicio.");
        }

        _context.Entry(horario).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict();
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Horario>> PostHorario(Horario horario)
    {
        if (!await _context.Publicaciones.AnyAsync(p => p.Id == horario.PublicacionId))
        {
            return NotFound($"La publicacion con id {horario.PublicacionId} no existe.");
        }

        if (horario.HoraFin <= horario.HoraInicio)
        {
            return BadRequest("La hora de fin debe ser posterior a la hora de inicio.");
        }

        _context.Horarios.Add(horario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetHorario), new { id = horario.Id }, horario);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHorario(int id)
    {
        var horario = await _context.Horarios.FindAsync(id);

        if (horario == null)
        {
            return NotFound();
        }

        _context.Horarios.Remove(horario);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}