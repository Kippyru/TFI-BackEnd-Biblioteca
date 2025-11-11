using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TFI_BackEnd_Biblioteca.Models;

namespace TFI_BackEnd_Biblioteca.Data;

public partial class BibliotecaContext : DbContext
{
    public BibliotecaContext()
    {
    }

    public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Editorial> Editorials { get; set; }

    public virtual DbSet<EjemplarLibro> EjemplarLibros { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<Libro> Libros { get; set; }

    public virtual DbSet<Prestamo> Prestamos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
       
        //Aca va la direccion del servidor SQL Server, la borre por cuestion de seguridad
        => optionsBuilder.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Editorial>(entity =>
        {
            entity.HasKey(e => e.IdEditorial).HasName("PK__Editoria__BCB52C78DB9FD25A");

            entity.ToTable("Editorial");

            entity.Property(e => e.IdEditorial).HasColumnName("ID_Editorial");
            entity.Property(e => e.Direccion)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EjemplarLibro>(entity =>
        {
            entity.HasKey(e => e.IdCodigoInventario).HasName("PK__Ejemplar__93ABE1269C33904B");

            entity.Property(e => e.IdCodigoInventario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Codigo_Inventario");
            entity.Property(e => e.CodigoUbicacion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Codigo_Ubicacion");
            entity.Property(e => e.FechaAlta).HasColumnName("Fecha_Alta");
            entity.Property(e => e.FechaBaja).HasColumnName("Fecha_Baja");
            entity.Property(e => e.IdPrestamo).HasColumnName("ID_Prestamo");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ISBN");

            entity.HasOne(d => d.IdPrestamoNavigation).WithMany(p => p.EjemplarLibros)
                .HasForeignKey(d => d.IdPrestamo)
                .HasConstraintName("FK_Ejemplar_Prestamo");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.EjemplarLibros)
                .HasForeignKey(d => d.Isbn)
                .HasConstraintName("FK_Ejemplar_Libro");
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.NLegajo).HasName("PK__Estudian__2640F0717EC29CE1");

            entity.ToTable("Estudiante");

            entity.Property(e => e.NLegajo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("N_Legajo");
            entity.Property(e => e.FechaIngresoFacultad).HasColumnName("Fecha_Ingreso_Facultad");
            entity.Property(e => e.MateriasCursando)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Materias_Cursando");
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(e => e.Isbn).HasName("PK__Libro__447D36EB1BCF494C");

            entity.ToTable("Libro");

            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ISBN");
            entity.Property(e => e.Autores)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Categoria)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaPublicacion).HasColumnName("Fecha_Publicacion");
            entity.Property(e => e.IdEditorial).HasColumnName("ID_Editorial");
            entity.Property(e => e.Titulo)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEditorialNavigation).WithMany(p => p.Libros)
                .HasForeignKey(d => d.IdEditorial)
                .HasConstraintName("FK_Libro_Editorial");
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.HasKey(e => e.IdPrestamo).HasName("PK__Prestamo__FE17DB17C18F2505");

            entity.Property(e => e.IdPrestamo).HasColumnName("ID_Prestamo");
            entity.Property(e => e.CodigoInventario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Codigo_Inventario");
            entity.Property(e => e.FechaDeFin).HasColumnName("Fecha_de_Fin");
            entity.Property(e => e.FechaDeInicio).HasColumnName("Fecha_de_Inicio");
            entity.Property(e => e.IdUser).HasColumnName("ID_User");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_Prestamo_Usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__DE4431C5E2126CBA");

            entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario");
            entity.Property(e => e.Direccion)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.NLegajo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("N_Legajo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.NLegajoNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.NLegajo)
                .HasConstraintName("FK_Usuario_Estudiante");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
