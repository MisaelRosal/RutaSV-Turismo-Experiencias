using IguanaSV.Api.Entities;
using IguanaSV.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IguanaSV.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagenesPublicacionController : ControllerBase
{
    private readonly IguanasDbContext _context;

    public ImagenesPublicacionController(IguanasDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ImagenesPublicacion>>> GetImagenesPublicacions()
    {
        return await _context.ImagenesPublicacions
            .Include(ip => ip.Publicacion)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ImagenesPublicacion>> GetImagenesPublicacion(int id)
    {
        var imagenesPublicacion = await _context.ImagenesPublicacions
            .Include(ip => ip.Publicacion)
            .FirstOrDefaultAsync(ip => ip.Id == id);

        if (imagenesPublicacion == null)
        {
            return NotFound();
        }

        return imagenesPublicacion;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutImagenesPublicacion(int id, ImagenesPublicacion imagenesPublicacion)
    {
        if (id != imagenesPublicacion.Id)
        {
            return BadRequest();
        }

        var exists = await _context.ImagenesPublicacions.AnyAsync(ip => ip.Id == id);

        if (!exists)
        {
            return NotFound();
        }

        if (!await _context.Publicaciones.AnyAsync(p => p.Id == imagenesPublicacion.PublicacionId))
        {
            return NotFound($"La publicacion con id {imagenesPublicacion.PublicacionId} no existe.");
        }

        _context.Entry(imagenesPublicacion).State = EntityState.Modified;

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
    public async Task<ActionResult<ImagenesPublicacion>> PostImagenesPublicacion(ImagenesPublicacion imagenesPublicacion)
    {
        if (!await _context.Publicaciones.AnyAsync(p => p.Id == imagenesPublicacion.PublicacionId))
        {
            return NotFound($"La publicacion con id {imagenesPublicacion.PublicacionId} no existe.");
        }

        _context.ImagenesPublicacions.Add(imagenesPublicacion);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetImagenesPublicacion), new { id = imagenesPublicacion.Id }, imagenesPublicacion);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteImagenesPublicacion(int id)
    {
        var imagenesPublicacion = await _context.ImagenesPublicacions.FindAsync(id);

        if (imagenesPublicacion == null)
        {
            return NotFound();
        }

        _context.ImagenesPublicacions.Remove(imagenesPublicacion);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}