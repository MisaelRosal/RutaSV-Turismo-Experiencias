using IguanaSV.Api.Entities;
using IguanaSV.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IguanaSV.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnfitrioneController : ControllerBase
{
    private readonly IguanasDbContext _context;

    public AnfitrioneController(IguanasDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Anfitrione>>> GetAnfitriones()
    {
        return await _context.Anfitriones
            .Include(a => a.Municipio)
            .Include(a => a.Publicaciones)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Anfitrione>> GetAnfitrione(int id)
    {
        var anfitrione = await _context.Anfitriones
            .Include(a => a.Municipio)
            .Include(a => a.Publicaciones)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (anfitrione == null)
        {
            return NotFound();
        }

        return anfitrione;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAnfitrione(int id, Anfitrione anfitrione)
    {
        if (id != anfitrione.Id)
        {
            return BadRequest();
        }

        var exists = await _context.Anfitriones.AnyAsync(a => a.Id == id);

        if (!exists)
        {
            return NotFound();
        }

        if (!await _context.Municipios.AnyAsync(m => m.Id == anfitrione.MunicipioId))
        {
            return NotFound($"El municipio con id {anfitrione.MunicipioId} no existe.");
        }

        _context.Entry(anfitrione).State = EntityState.Modified;

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
    public async Task<ActionResult<Anfitrione>> PostAnfitrione(Anfitrione anfitrione)
    {
        if (!await _context.Municipios.AnyAsync(m => m.Id == anfitrione.MunicipioId))
        {
            return NotFound($"El municipio con id {anfitrione.MunicipioId} no existe.");
        }

        _context.Anfitriones.Add(anfitrione);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAnfitrione), new { id = anfitrione.Id }, anfitrione);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnfitrione(int id)
    {
        var anfitrione = await _context.Anfitriones.FindAsync(id);

        if (anfitrione == null)
        {
            return NotFound();
        }

        _context.Anfitriones.Remove(anfitrione);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}