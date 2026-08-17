using Microsoft.AspNetCore.Mvc;
using Npgsql;

[ApiController]
[Route("api/[controller]")]
public class MunicipiosController : ControllerBase
{
    private readonly IConfiguration configuration;

    public MunicipiosController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var connStr = configuration.GetConnectionString("DefaultConnection");
        var lista = new List<object>();

        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand("SELECT * FROM municipios ORDER BY id", conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new
            {
                Id = reader.GetInt32(0),
                Nombre = reader.IsDBNull(1) ? null : reader.GetString(1),
                Departamento_id = reader.GetInt32(2),
                CreatedAt = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                UpdatedAt = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
            });
        }

        return Ok(lista);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var connStr = configuration.GetConnectionString("DefaultConnection");

        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "SELECT id, nombre, departamento_id, created_at, updated_at FROM municipios WHERE id = @id",
            conn);

        cmd.Parameters.AddWithValue("id", id);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var municipio = new
            {
                Id = reader.GetInt32(0),
                Nombre = reader.IsDBNull(1) ? null : reader.GetString(1),
                Departamento_id = reader.GetInt32(2),
                CreatedAt = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                UpdatedAt = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
            };

            return Ok(municipio);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Municipio municipio)
    {
        var connStr = configuration.GetConnectionString("DefaultConnection");

        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "INSERT INTO municipios (nombre, departamento_id, created_at, updated_at) VALUES (@nombre, @departamento_id, @created_at, @updated_at) RETURNING id",
            conn);

        cmd.Parameters.AddWithValue("nombre", municipio.Nombre);
        cmd.Parameters.AddWithValue("departamento_id", municipio.Departamento_id);
        cmd.Parameters.AddWithValue("created_at", DateTime.UtcNow);
        cmd.Parameters.AddWithValue("updated_at", DateTime.UtcNow);

        var newId = (int)await cmd.ExecuteScalarAsync();

        return CreatedAtAction(nameof(GetById), new { id = newId }, new { Id = newId });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Municipio municipio)
    {
        var connStr = configuration.GetConnectionString("DefaultConnection");

        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        var now = DateTime.UtcNow;

        await using var cmd = new NpgsqlCommand(
            "UPDATE municipios SET nombre = @nombre, departamento_id = @departamento_id, created_at = @created_at, updated_at = @updated_at WHERE id = @id",
            conn);

        cmd.Parameters.AddWithValue("id", id);
        cmd.Parameters.AddWithValue("nombre", municipio.Nombre);
        cmd.Parameters.AddWithValue("departamento_id", municipio.Departamento_id);
        cmd.Parameters.AddWithValue("created_at", municipio.CreatedAt ?? now);
        cmd.Parameters.AddWithValue("updated_at", now);

        var rowsAffected = await cmd.ExecuteNonQueryAsync();
        if (rowsAffected == 0)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var connStr = configuration.GetConnectionString("DefaultConnection");

        await using var conn = new NpgsqlConnection(connStr);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand("DELETE FROM municipios WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("id", id);

        var rowsAffected = await cmd.ExecuteNonQueryAsync();
        if (rowsAffected == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
}