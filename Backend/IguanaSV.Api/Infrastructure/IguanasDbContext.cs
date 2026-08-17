using Microsoft.EntityFrameworkCore;
using IguanaSV.Api.Entities;

namespace IguanaSV.Api.Infrastructure
{
    public class IguanasDbContext : DbContext
    {
        public IguanasDbContext(DbContextOptions<IguanasDbContext> options)
            : base(options)
        {
        }

        // DbSets para cada tabla
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Anfitrione> Anfitriones { get; set; }
        public DbSet<Publicacione> Publicaciones { get; set; }
        public DbSet<Amenidade> Amenidades { get; set; }
        public DbSet<PublicacionAmenidad> PublicacionAmenidades { get; set; }
        public DbSet<ImagenesPublicacion> ImagenesPublicacion { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<ReservaHorario> ReservaHorarios { get; set; }
        public DbSet<Experiencia> Experiencias { get; set; }
        public DbSet<Notificacione> Notificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            // 1. Índice de exclusión para Horarios (evita solapamiento)
            modelBuilder.Entity<Horario>()
                .HasIndex(h => new { h.PublicacionId, h.DiaSemana })
                .HasMethod("gist")
                .HasDatabaseName("horarios_no_overlap");

            // NOTA: La restricción EXCLUDE con tsrange debe agregarse manualmente
            // en una migración con SQL puro. Ver el archivo de migración.

            // 2. Índice de exclusión para Reservas (evita doble reserva)
            modelBuilder.Entity<Reserva>()
                .HasIndex(r => r.PublicacionId)
                .HasMethod("gist")
                .HasDatabaseName("reservas_no_overlap");

            // NOTA: La restricción EXCLUDE con daterange debe agregarse manualmente
            // en una migración con SQL puro.

            // =============================================
            // CONFIGURACIONES ADICIONALES (Opcional)
            // =============================================
            
            // Ejemplo: Configurar precisión de decimales
            modelBuilder.Entity<Publicacione>()
                .Property(p => p.PrecioPorNoche)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Reserva>()
                .Property(r => r.PrecioTotal)
                .HasPrecision(10, 2);

            // Ejemplo: Índices adicionales para rendimiento
            modelBuilder.Entity<Municipio>()
                .HasIndex(m => m.DepartamentoId)
                .HasDatabaseName("idx_municipios_departamento");

            modelBuilder.Entity<Anfitrione>()
                .HasIndex(a => a.MunicipioId)
                .HasDatabaseName("idx_anfitriones_municipio");

            modelBuilder.Entity<Publicacione>()
                .HasIndex(p => p.AnfitrionId)
                .HasDatabaseName("idx_publicaciones_anfitrion");

            modelBuilder.Entity<Publicacione>()
                .HasIndex(p => p.CategoriaId)
                .HasDatabaseName("idx_publicaciones_categoria");

            modelBuilder.Entity<Publicacione>()
                .HasIndex(p => p.Estado)
                .HasDatabaseName("idx_publicaciones_estado");

            modelBuilder.Entity<Reserva>()
                .HasIndex(r => r.PublicacionId)
                .HasDatabaseName("idx_reservas_publicacion");

            modelBuilder.Entity<Reserva>()
                .HasIndex(r => new { r.FechaInicio, r.FechaFin })
                .HasDatabaseName("idx_reservas_fechas");
        }
    }
}