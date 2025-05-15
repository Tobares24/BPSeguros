using Common.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Poliza.Entities
{
    public class PolizaDbContext : DbContext
    {
        public PolizaDbContext() : base() { }

        public PolizaDbContext(DbContextOptions<PolizaDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                SqlConnection sqlConnection = new();
                var connectionString = Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING") ?? "Server=localhost;Database=PolizaDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
                optionsBuilder.UseSqlServer(sqlConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PolizaEntity>()
                .HasKey(x => x.Id)
                .HasName("PK_Poliza_Id");

            modelBuilder.Entity<PolizaCoberturaEntity>()
                .HasKey(x => x.Id)
                .HasName("PK_PolizaCobertura_Id");

            modelBuilder.Entity<PolizaEstadoEntity>()
                .HasKey(x => x.Id)
                .HasName("PK_PolizaEstado_Id");

            modelBuilder.Entity<PolizaEntity>()
                .HasOne(x => x.PolizaCobertura)
                .WithMany(x => x.Polizas)
                .HasForeignKey(x => x.IdCobertura)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Poliza_PolizaCobertura");

            modelBuilder.Entity<PolizaEntity>()
                .HasOne(x => x.TipoPoliza)
                .WithMany(x => x.Polizas)
                .HasForeignKey(x => x.IdTipoPoliza)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Poliza_TipoPoliza");

            modelBuilder.Entity<PolizaEntity>()
                .HasOne(x => x.PolizaEstado)
                .WithMany(x => x.Polizas)
                .HasForeignKey(x => x.IdPolizaEstado)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Poliza_PolizaEstado");
        }

        public DbSet<PolizaEntity> Poliza { get; set; }
        public DbSet<PolizaCoberturaEntity> PolizaCobertura { get; set; }
        public DbSet<TipoPolizaEntity> TipoPoliza { get; set; }
        public DbSet<PolizaEstadoEntity> PolizaEstado { get; set; }
        public DbSet<PolizaPeriodoEntity> PolizaPeriodo { get; set; }
    }
}