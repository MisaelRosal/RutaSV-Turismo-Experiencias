using IguanaSV.Api.Entities;
using IguanaSV.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IguanaSV.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MunicipioController : ControllerBase
{
    private readonly IguanasDbContext _context;

    public MunicipioController(IguanasDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Municipio>>> GetMunicipios()
    {
        return await _context.Municipios
            .Include(m => m.Departamento)
            .Include(m => m.Anfitriones)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Municipio>> GetMunicipio(int id)
    {
        var municipio = await _context.Municipios
            .Include(m => m.Departamento)
            .Include(m => m.Anfitriones)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (municipio == null)
        {
            return NotFound();
        }

        return municipio;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMunicipio(int id, Municipio municipio)
    {
        if (id != municipio.Id)
        {
            return BadRequest();
        }

        var exists = await _context.Municipios.AnyAsync(m => m.Id == id);

        if (!exists)
        {
            return NotFound();
        }

        if (!await _context.Departamentos.AnyAsync(d => d.Id == municipio.DepartamentoId))
        {
            return NotFound($"El departamento con id {municipio.DepartamentoId} no existe.");
        }

        _context.Entry(municipio).State = EntityState.Modified;

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
    public async Task<ActionResult<Municipio>> PostMunicipio(Municipio municipio)
    {
        if (!await _context.Departamentos.AnyAsync(d => d.Id == municipio.DepartamentoId))
        {
            return NotFound($"El departamento con id {municipio.DepartamentoId} no existe.");
        }

        _context.Municipios.Add(municipio);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMunicipio), new { id = municipio.Id }, municipio);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMunicipio(int id)
    {
        var municipio = await _context.Municipios.FindAsync(id);

        if (municipio == null)
        {
            return NotFound();
        }

        _context.Municipios.Remove(municipio);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}