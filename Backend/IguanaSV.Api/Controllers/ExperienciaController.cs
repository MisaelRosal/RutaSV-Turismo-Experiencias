using IguanaSV.Api.Entities;
using IguanaSV.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IguanaSV.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExperienciaController : ControllerBase
{
    private readonly IguanasDbContext _context;

    public ExperienciaController(IguanasDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Experiencia>>> GetExperiencias()
    {
        return await _context.Experiencias
            .Include(e => e.Publicacion)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Experiencia>> GetExperiencia(int id)
    {
        var experiencia = await _context.Experiencias
            .Include(e => e.Publicacion)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (experiencia == null)
        {
            return NotFound();
        }

        return experiencia;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutExperiencia(int id, Experiencia experiencia)
    {
        if (id != experiencia.Id)
        {
            return BadRequest();
        }

        var exists = await _context.Experiencias.AnyAsync(e => e.Id == id);

        if (!exists)
        {
            return NotFound();
        }

        if (!await _context.Publicaciones.AnyAsync(p => p.Id == experiencia.PublicacionId))
        {
            return NotFound($"La publicacion con id {experiencia.PublicacionId} no existe.");
        }

        if (experiencia.DuracionHoras.HasValue && experiencia.DuracionHoras <= 0)
        {
            return BadRequest("La duracion en horas debe ser mayor a 0.");
        }

        if (experiencia.PrecioAdicional.HasValue && experiencia.PrecioAdicional < 0)
        {
            return BadRequest("El precio adicional no puede ser negativo.");
        }

        _context.Entry(experiencia).State = EntityState.Modified;

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
    public async Task<ActionResult<Experiencia>> PostExperiencia(Experiencia experiencia)
    {
        if (!await _context.Publicaciones.AnyAsync(p => p.Id == experiencia.PublicacionId))
        {
            return NotFound($"La publicacion con id {experiencia.PublicacionId} no existe.");
        }

        if (experiencia.DuracionHoras.HasValue && experiencia.DuracionHoras <= 0)
        {
            return BadRequest("La duracion en horas debe ser mayor a 0.");
        }

        if (experiencia.PrecioAdicional.HasValue && experiencia.PrecioAdicional < 0)
        {
            return BadRequest("El precio adicional no puede ser negativo.");
        }

        _context.Experiencias.Add(experiencia);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetExperiencia), new { id = experiencia.Id }, experiencia);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExperiencia(int id)
    {
        var experiencia = await _context.Experiencias.FindAsync(id);

        if (experiencia == null)
        {
            return NotFound();
        }

        _context.Experiencias.Remove(experiencia);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}