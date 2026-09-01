using ExpenSysCom.Modelos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenSysCom.Data;

public partial class ExpenSysDBContext : IdentityDbContext<ApplicationUser>
{
    public ExpenSysDBContext(DbContextOptions<ExpenSysDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ahorro> Ahorros { get; set; } = null!;
    public virtual DbSet<AportesAhorro> AportesAhorros { get; set; } = null!;
    public virtual DbSet<CategoriasGasto> CategoriasGastos { get; set; } = null!;
    public virtual DbSet<CategoriasIngreso> CategoriasIngresos { get; set; } = null!;
    public virtual DbSet<Estado> Estados { get; set; } = null!;
    public virtual DbSet<Gasto> Gastos { get; set; } = null!;
    public virtual DbSet<Ingreso> Ingresos { get; set; } = null!;
    public virtual DbSet<Intensidad> Intensidades { get; set; } = null!;
    public virtual DbSet<Meta> Metas { get; set; } = null!;
    public virtual DbSet<GastosAhorro> GastosAhorros { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("AspNetUsers");
            entity.Property(e => e.Id).HasColumnName("ID_Usuario");
        });

        modelBuilder.Entity<Ahorro>(entity =>
        {
            entity.HasKey(e => e.IdAhorro).HasName("PK_Ahorro");
            entity.Property(e => e.IdAhorro).HasColumnName("ID_Ahorro");
            entity.Property(e => e.Nombre).HasMaxLength(250).IsUnicode(false);
            entity.Property(e => e.Descripcion).HasMaxLength(250).IsUnicode(false).IsRequired(false);
            entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario");
            entity.Property(e => e.IdEstado).HasColumnName("ID_Estado");
            entity.Property(e => e.IdIntensidad).HasColumnName("ID_Intensidad");

            entity.HasOne(e => e.IdUsuarioNavigation)
                .WithMany()
                .HasForeignKey(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdEstadoNavigation)
                .WithMany(p => p.Ahorros)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ahorro_Estado");

            entity.HasOne(d => d.IdIntensidadNavigation)
                .WithMany(p => p.Ahorros)
                .HasForeignKey(d => d.IdIntensidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ahorro_Intensidad");
        });


        modelBuilder.Entity<Ingreso>(entity =>
        {
            entity.HasKey(e => e.IdIngreso).HasName("PK_Ingresos");
            entity.Property(e => e.IdIngreso).HasColumnName("ID_Ingreso");
            entity.Property(e => e.Descripcion).HasMaxLength(250).IsUnicode(false);
            entity.Property(e => e.Fecha).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e => e.IdCategoriaIngreso).HasColumnName("ID_CategoriaIngreso");
            entity.Property(e => e.Nombre).HasMaxLength(250).IsUnicode(false);
            entity.Property(e => e.Valor).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario");

            entity.HasOne(e => e.IdUsuarioNavigation)
                .WithMany()
                .HasForeignKey(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdCategoriaIngresoNavigation)
                .WithMany(p => p.Ingresos)
                .HasForeignKey(d => d.IdCategoriaIngreso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ingresos_CategoriasIngresos");
        });

        modelBuilder.Entity<Meta>(entity =>
        {
            entity.HasKey(e => e.IdMeta).HasName("PK_Meta");
            entity.Property(e => e.IdMeta).HasColumnName("ID_Meta");
            entity.Property(e => e.Nombre).HasMaxLength(250).IsUnicode(false);
            entity.Property(e => e.Descripcion).HasMaxLength(250).IsUnicode(false);
            entity.Property(e => e.Valor).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario");
            entity.Property(e => e.IdAhorro).HasColumnName("ID_Ahorro");
            entity.Property(e => e.IdEstado).HasColumnName("ID_Estado");

            entity.HasOne(e => e.IdUsuarioNavigation)
                .WithMany()
                .HasForeignKey(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdAhorroNavigation)
                .WithMany(p => p.Meta)
                .HasForeignKey(d => d.IdAhorro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Meta_Ahorro");

            entity.HasOne(d => d.IdEstadoNavigation)
                .WithMany(p => p.Meta)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Meta_Estado");
        });

        modelBuilder.Entity<Gasto>(entity =>
        {
            entity.HasKey(e => e.IdGasto).HasName("PK_Gastos");
            entity.Property(e => e.IdGasto).HasColumnName("ID_Gasto");
            entity.Property(e => e.Descripcion).HasMaxLength(250).IsUnicode(false);
            entity.Property(e => e.Fecha).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e => e.IdCategoriaGasto).HasColumnName("ID_CategoriaGasto");
            entity.Property(e => e.Nombre).HasMaxLength(250).IsUnicode(false);
            entity.Property(e => e.Valor).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario");

            entity.HasOne(g => g.IdUsuarioNavigation)
                .WithMany()
                .HasForeignKey(g => g.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdCategoriaGastoNavigation)
                  .WithMany(p => p.Gastos)
                  .HasForeignKey(d => d.IdCategoriaGasto)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_Gastos_CategoriasGastos");
        });

        modelBuilder.Entity<GastosAhorro>(entity =>
        {
            entity.HasKey(e => e.IdGastoAhorro).HasName("PK_GastosAhorro"); 
            entity.Property(e => e.IdGastoAhorro).HasColumnName("ID_GastoAhorro");
            entity.Property(e => e.Valor).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Fecha).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IdAhorro).HasColumnName("ID_Ahorro");
            entity.Property(e => e.IdGasto).HasColumnName("ID_Gasto");

            entity.HasOne(d => d.IdAhorroNavigation)
                .WithMany(p => p.GastosAhorros)
                .HasForeignKey(d => d.IdAhorro)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GastosAhorro_Ahorro");

            entity.HasOne(d => d.IdGastoNavigation)
                .WithMany(p => p.GastosAhorros)
                .HasForeignKey(d => d.IdGasto)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GastosAhorro_Gasto");
        });

        modelBuilder.Entity<AportesAhorro>(entity =>
        {
            entity.HasKey(e => e.IdAporteAhorro).HasName("PK_AportesAhorros");
            entity.Property(e => e.IdAporteAhorro).HasColumnName("ID_AporteAhorro");
            entity.Property(e => e.Fecha).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e => e.IdAhorro).HasColumnName("ID_Ahorro");
            entity.Property(e => e.IdIngreso).HasColumnName("ID_Ingreso");
            entity.Property(e => e.Valor).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdAhorroNavigation)
                .WithMany(p => p.AportesAhorros)
                .HasForeignKey(d => d.IdAhorro)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_AportesAhorros_Ahorros");

            entity.HasOne(d => d.IdIngresoNavigation)
                .WithMany(p => p.AportesAhorros)
                .HasForeignKey(d => d.IdIngreso)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_AportesAhorros_Ingresos");
        });


        modelBuilder.Entity<CategoriasGasto>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaGasto).HasName("PK_CategoriasGasto");
            entity.Property(e => e.IdCategoriaGasto).HasColumnName("ID_CategoriasGasto");
            entity.Property(e => e.Nombre).HasMaxLength(250).IsUnicode(false);
        });

        modelBuilder.Entity<CategoriasIngreso>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaIngreso).HasName("PK_CategoriasIngreso");
            entity.Property(e => e.IdCategoriaIngreso).HasColumnName("ID_CategoriasIngreso");
            entity.Property(e => e.Nombre).HasMaxLength(250).IsUnicode(false);
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK_Estados");
            entity.Property(e => e.IdEstado).HasColumnName("ID_Estado");
            entity.Property(e => e.Nombre).HasMaxLength(250).IsUnicode(false);
        });

        modelBuilder.Entity<Intensidad>(entity =>
        {
            entity.HasKey(e => e.IdIntensidad).HasName("PK_Intensidades");
            entity.Property(e => e.IdIntensidad).HasColumnName("ID_Intensidad");
            entity.Property(e => e.Nombre).HasMaxLength(250).IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
