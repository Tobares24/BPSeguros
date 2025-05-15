using Common.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Seguridad.Entities
{
    public class SeguridadDbContext : DbContext
    {
        public SeguridadDbContext() : base() { }

        public SeguridadDbContext(DbContextOptions<SeguridadDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                SqlConnection sqlConnection = new();
                var connectionString = Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING") ?? "Server=localhost;Database=SeguridadDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
                optionsBuilder.UseSqlServer(sqlConnection);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioEntity>()
              .HasKey(x => x.Id)
              .HasName("PK_Usuario_Id");
        }

        public DbSet<UsuarioEntity> Usuario { get; set; }
    }
}
