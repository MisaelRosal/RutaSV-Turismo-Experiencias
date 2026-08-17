using IguanaSV.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IguanaSV.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificacioneController : ControllerBase
{
    private readonly IguanaContext _context;

    public NotificacioneController(IguanaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Notificacione>>> GetNotificaciones()
    {
        return await _context.Notificaciones
            .Include(n => n.Reserva)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Notificacione>> GetNotificacione(int id)
    {
        var notificacione = await _context.Notificaciones
            .Include(n => n.Reserva)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (notificacione == null)
        {
            return NotFound();
        }

        return notificacione;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutNotificacione(int id, Notificacione notificacione)
    {
        if (id != notificacione.Id)
        {
            return BadRequest();
        }

        var exists = await _context.Notificaciones.AnyAsync(n => n.Id == id);

        if (!exists)
        {
            return NotFound();
        }

        if (!await _context.Reservas.AnyAsync(r => r.Id == notificacione.ReservaId))
        {
            return NotFound($"La reserva con id {notificacione.ReservaId} no existe.");
        }

        _context.Entry(notificacione).State = EntityState.Modified;

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
    public async Task<ActionResult<Notificacione>> PostNotificacione(Notificacione notificacione)
    {
        if (!await _context.Reservas.AnyAsync(r => r.Id == notificacione.ReservaId))
        {
            return NotFound($"La reserva con id {notificacione.ReservaId} no existe.");
        }

        _context.Notificaciones.Add(notificacione);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNotificacione), new { id = notificacione.Id }, notificacione);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotificacione(int id)
    {
        var notificacione = await _context.Notificaciones.FindAsync(id);

        if (notificacione == null)
        {
            return NotFound();
        }

        _context.Notificaciones.Remove(notificacione);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}