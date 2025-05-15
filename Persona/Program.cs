using Common.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persona.Entities;
using Persona.Services.ActualizarPersona;
using Persona.Services.CrearPersona;
using Persona.Services.DataInicial;
using Persona.Services.EliminarPersona;
using Persona.Services.ListaSelectorPersona;
using Persona.Services.ListaSelectorTipoPersona;
using Persona.Services.ObtenerPersona;
using Persona.Services.ObtenerPorId;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DbContextFactoryService>();
builder.Services.AddTransient<RequestModelValidationService>();
builder.Services.AddTransient<JsonService>();
builder.Services.AddTransient<CrearPersonaService>();
builder.Services.AddTransient<ObtenerPersonaService>();
builder.Services.AddTransient<EliminarPersonaService>();
builder.Services.AddTransient<ActualizarPersonaService>();
builder.Services.AddTransient<ListaSelectorPersonaService>();
builder.Services.AddTransient<ListaSelectorTipoPersonaService>();
builder.Services.AddTransient<DataInicialService>();
builder.Services.AddTransient<ObtenerPorIdService>();
builder.Services.AddDbContext<PersonaDbContext>(options =>
{
    SqlConnection sqlConnection = new();
    sqlConnection.ConnectionString = Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING") ?? "Server=localhost;Database=PersonaDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
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

using (var dbContext = app.Services.GetRequiredService<DbContextFactoryService>().CreateDbContext<PersonaDbContext>())
{
    await dbContext.Database.EnsureCreatedAsync();
    await dbContext.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseCors("*");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

_ = Task.Run(async () =>
{
    DataInicialService service = app.Services.GetService<DataInicialService>()!;
    await service.CrearTipoPersonas();
});

app.Run();
