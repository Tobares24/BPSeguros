using Common.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Persona.Entities;
using Persona.Services.ActualizarPersona;
using Persona.Services.CrearPersona;
using Persona.Services.EliminarPersona;
using Persona.Services.ListaSelectorPersona;
using Persona.Services.ObtenerPersona;

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
builder.Services.AddDbContext<PersonaDbContext>(options =>
{
    SqlConnection sqlConnection = new();
    sqlConnection.ConnectionString = Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING") ?? "Server=localhost;Database=PersonaDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
    options.UseSqlServer(sqlConnection);
});

var app = builder.Build();

using (var dbContext = app.Services.GetRequiredService<DbContextFactoryService>().CreateDbContext<PersonaDbContext>())
{
    await dbContext.Database.EnsureCreatedAsync();
    await dbContext.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
