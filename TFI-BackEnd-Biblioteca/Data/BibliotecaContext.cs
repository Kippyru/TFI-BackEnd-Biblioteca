using Microsoft.EntityFrameworkCore;
using TFI_BackEnd_Biblioteca.Models;

namespace TFI_BackEnd_Biblioteca.Data
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) 
    : base(options)
        {
  }

        // DbSets para las entidades
 public DbSet<Libro> Libros { get; set; }
        public DbSet<Socio> Socios { get; set; }
  public DbSet<Prestamo> Prestamos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        base.OnModelCreating(modelBuilder);

            // Configuraciones adicionales de las entidades
            
       // Configuración de Libro
   modelBuilder.Entity<Libro>(entity =>
            {
 entity.HasKey(e => e.Id);
     entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Autor).IsRequired().HasMaxLength(150);
          entity.Property(e => e.ISBN).HasMaxLength(13);
            });

            // Configuración de Socio
        modelBuilder.Entity<Socio>(entity =>
  {
              entity.HasKey(e => e.Id);
       entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
       entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
 entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
    entity.HasIndex(e => e.Email).IsUnique();
            });

    // Configuración de Prestamo
            modelBuilder.Entity<Prestamo>(entity =>
            {
         entity.HasKey(e => e.Id);
            
   // Relación con Libro
      entity.HasOne(e => e.Libro)
      .WithMany(l => l.Prestamos)
          .HasForeignKey(e => e.LibroId)
               .OnDelete(DeleteBehavior.Restrict);

    // Relación con Socio
      entity.HasOne(e => e.Socio)
      .WithMany(s => s.Prestamos)
        .HasForeignKey(e => e.SocioId)
             .OnDelete(DeleteBehavior.Restrict);
  });
    }
    }
}
