using Common.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Seguridad.Entities;
using Seguridad.Services.CrearUsuario;
using Seguridad.Services.Login;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<JWTService>();
builder.Services.AddSingleton<DbContextFactoryService>();
builder.Services.AddTransient<PasswordHasherService>();
builder.Services.AddTransient<CrearUsuarioService>();
builder.Services.AddTransient<RequestModelValidationService>();
builder.Services.AddTransient<JsonService>();
builder.Services.AddTransient<LoginService>();
builder.Services.AddDbContext<SeguridadDbContext>(options =>
{
    SqlConnection sqlConnection = new SqlConnection();
    sqlConnection.ConnectionString = Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING") ?? "Server=localhost;Database=SeguridadDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
    options.UseSqlServer(sqlConnection);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("*", policy =>
    {
        policy
            .SetIsOriginAllowed(origin => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Environment.GetEnvironmentVariable("ISSUER"),
            ValidAudience = Environment.GetEnvironmentVariable("AUDIENCE"),
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET_KEY")!))
        };
    });

var app = builder.Build();

using (var dbContext = app.Services.GetRequiredService<DbContextFactoryService>().CreateDbContext<SeguridadDbContext>())
{
    try
    {
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Database.MigrateAsync();
    }
    catch (Exception) { }
}

_ = Task.Run(async () =>
{
    using (var dbContext = app.Services.GetRequiredService<DbContextFactoryService>().CreateDbContext<SeguridadDbContext>())
    {
        try
        {
            var migrationsToMark = new[]
            {
               "20250515114533_202505132330",
            };

            foreach (var migrationId in migrationsToMark)
            {
                var sql = $"INSERT INTO [__EFMigrationsHistory] (MigrationId, ProductVersion) VALUES ('{migrationId}', '7.0.0');";
                await dbContext.Database.ExecuteSqlRawAsync(sql);
            }
        }
        catch (Exception) { }
    }
});

app.UseCors("*");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
