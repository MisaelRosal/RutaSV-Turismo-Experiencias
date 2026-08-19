using Microsoft.EntityFrameworkCore;

public class RutaDbContext : DbContext
{
    public RutaDbContext(DbContextOptions<RutaDbContext> options) : base(options)
    {
    }
}