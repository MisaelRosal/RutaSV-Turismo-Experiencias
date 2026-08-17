using IguanaSV.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IguanaSV.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublicacionAmenidadController : ControllerBase
{
    private readonly IguanaContext _context;

    public PublicacionAmenidadController(IguanaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PublicacionAmenidad>>> GetPublicacionAmenidads()
    {
        return await _context.PublicacionAmenidads
            .Include(pa => pa.Publicacion)
            .Include(pa => pa.Amenidad)
            .ToListAsync();
    }

    [HttpGet("{publicacionId}/{amenidadId}")]
    public async Task<ActionResult<PublicacionAmenidad>> GetPublicacionAmenidad(int publicacionId, int amenidadId)
    {
        var publicacionAmenidad = await _context.PublicacionAmenidads
            .Include(pa => pa.Publicacion)
            .Include(pa => pa.Amenidad)
            .FirstOrDefaultAsync(pa => pa.PublicacionId == publicacionId && pa.AmenidadId == amenidadId);

        if (publicacionAmenidad == null)
        {
            return NotFound();
        }

        return publicacionAmenidad;
    }

    [HttpPut("{publicacionId}/{amenidadId}")]
    public async Task<IActionResult> PutPublicacionAmenidad(int publicacionId, int amenidadId, PublicacionAmenidad publicacionAmenidad)
    {
        if (publicacionId != publicacionAmenidad.PublicacionId || amenidadId != publicacionAmenidad.AmenidadId)
        {
            return BadRequest();
        }

        var exists = await _context.PublicacionAmenidads
            .AnyAsync(pa => pa.PublicacionId == publicacionId && pa.AmenidadId == amenidadId);

        if (!exists)
        {
            return NotFound();
        }

        _context.Entry(publicacionAmenidad).State = EntityState.Modified;

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
    public async Task<ActionResult<PublicacionAmenidad>> PostPublicacionAmenidad(PublicacionAmenidad publicacionAmenidad)
    {
        var publicacionExists = await _context.Publicaciones.AnyAsync(p => p.Id == publicacionAmenidad.PublicacionId);
        if (!publicacionExists)
        {
            return NotFound($"La publicacion con id {publicacionAmenidad.PublicacionId} no existe.");
        }

        var amenidadExists = await _context.Amenidades.AnyAsync(a => a.Id == publicacionAmenidad.AmenidadId);
        if (!amenidadExists)
        {
            return NotFound($"La amenidad con id {publicacionAmenidad.AmenidadId} no existe.");
        }

        var alreadyExists = await _context.PublicacionAmenidads
            .AnyAsync(pa => pa.PublicacionId == publicacionAmenidad.PublicacionId && pa.AmenidadId == publicacionAmenidad.AmenidadId);

        if (alreadyExists)
        {
            return Conflict("Esa relacion publicacion-amenidad ya existe.");
        }

        _context.PublicacionAmenidads.Add(publicacionAmenidad);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPublicacionAmenidad), new { publicacionId = publicacionAmenidad.PublicacionId, amenidadId = publicacionAmenidad.AmenidadId }, publicacionAmenidad);
    }

    [HttpDelete("{publicacionId}/{amenidadId}")]
    public async Task<IActionResult> DeletePublicacionAmenidad(int publicacionId, int amenidadId)
    {
        var publicacionAmenidad = await _context.PublicacionAmenidads
            .FirstOrDefaultAsync(pa => pa.PublicacionId == publicacionId && pa.AmenidadId == amenidadId);

        if (publicacionAmenidad == null)
        {
            return NotFound();
        }

        _context.PublicacionAmenidads.Remove(publicacionAmenidad);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}