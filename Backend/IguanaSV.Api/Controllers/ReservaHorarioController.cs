using IguanaSV.Api.Entities;
using IguanaSV.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IguanaSV.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservaHorarioController : ControllerBase
{
    private readonly IguanasDbContext _context;

    public ReservaHorarioController(IguanasDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservaHorario>>> GetReservaHorarios()
    {
        return await _context.ReservaHorarios
            .Include(rh => rh.Reserva)
            .Include(rh => rh.Horario)
            .ToListAsync();
    }

    [HttpGet("{reservaId}/{horarioId}")]
    public async Task<ActionResult<ReservaHorario>> GetReservaHorario(int reservaId, int horarioId)
    {
        var reservaHorario = await _context.ReservaHorarios
            .Include(rh => rh.Reserva)
            .Include(rh => rh.Horario)
            .FirstOrDefaultAsync(rh => rh.ReservaId == reservaId && rh.HorarioId == horarioId);

        if (reservaHorario == null)
        {
            return NotFound();
        }

        return reservaHorario;
    }

    [HttpPut("{reservaId}/{horarioId}")]
    public async Task<IActionResult> PutReservaHorario(int reservaId, int horarioId, ReservaHorario reservaHorario)
    {
        if (reservaId != reservaHorario.ReservaId || horarioId != reservaHorario.HorarioId)
        {
            return BadRequest();
        }

        var exists = await _context.ReservaHorarios
            .AnyAsync(rh => rh.ReservaId == reservaId && rh.HorarioId == horarioId);

        if (!exists)
        {
            return NotFound();
        }

        _context.Entry(reservaHorario).State = EntityState.Modified;

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
    public async Task<ActionResult<ReservaHorario>> PostReservaHorario(ReservaHorario reservaHorario)
    {
        var reservaExists = await _context.Reservas.AnyAsync(r => r.Id == reservaHorario.ReservaId);
        if (!reservaExists)
        {
            return NotFound($"La reserva con id {reservaHorario.ReservaId} no existe.");
        }

        var horarioExists = await _context.Horarios.AnyAsync(h => h.Id == reservaHorario.HorarioId);
        if (!horarioExists)
        {
            return NotFound($"El horario con id {reservaHorario.HorarioId} no existe.");
        }

        var alreadyExists = await _context.ReservaHorarios
            .AnyAsync(rh => rh.ReservaId == reservaHorario.ReservaId && rh.HorarioId == reservaHorario.HorarioId);

        if (alreadyExists)
        {
            return Conflict("Esa relacion reserva-horario ya existe.");
        }

        _context.ReservaHorarios.Add(reservaHorario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetReservaHorario), new { reservaId = reservaHorario.ReservaId, horarioId = reservaHorario.HorarioId }, reservaHorario);
    }

    [HttpDelete("{reservaId}/{horarioId}")]
    public async Task<IActionResult> DeleteReservaHorario(int reservaId, int horarioId)
    {
        var reservaHorario = await _context.ReservaHorarios
            .FirstOrDefaultAsync(rh => rh.ReservaId == reservaId && rh.HorarioId == horarioId);

        if (reservaHorario == null)
        {
            return NotFound();
        }

        _context.ReservaHorarios.Remove(reservaHorario);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}