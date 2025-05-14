using Common.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Persona.Entities
{
    public class PersonaDbContext : DbContext
    {
        public PersonaDbContext() : base() { }

        public PersonaDbContext(DbContextOptions<PersonaDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                SqlConnection sqlConnection = new();
                sqlConnection.ConnectionString = Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING") ?? "Server=localhost;Database=BPSeguros;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
                optionsBuilder.UseSqlServer(sqlConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonaEntity>()
                .HasKey(x => x.CedulaAsegurado)
                .HasName("PK_Persona_CedulaAsegurado");

            modelBuilder.Entity<TipoPersonaEntity>()
                .HasKey(x => x.Id)
                .HasName("PK_TipoPersona_Id");

            modelBuilder.Entity<PersonaEntity>()
                .HasOne(x => x.TipoPersona)
                .WithOne(x => x.Persona)
                .HasForeignKey<PersonaEntity>(x => x.IdTipoPersona)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Persona_TipoPersona");
        }

        public DbSet<PersonaEntity> Persona { get; set; }
        public DbSet<TipoPersonaEntity> TipoPersona { get; set; }
    }
}