using Common.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Poliza.Entities;
using Poliza.Services;
using Poliza.Services.ActualizarPoliza;
using Poliza.Services.DataInicial;
using Poliza.Services.EliminarPoliza;
using Poliza.Services.ListaSelectorPolizaCobertura;
using Poliza.Services.ListaSelectorPolizaEstado;
using Poliza.Services.ListaSelectorTipoPoliza;
using Poliza.Services.ObtenerPoliza;
using Poliza.Services.ObtenerPorId;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddSingleton<DbContextFactoryService>();
builder.Services.AddTransient<APIService>();
builder.Services.AddTransient<InternalService>();
builder.Services.AddTransient<RequestModelValidationService>();
builder.Services.AddTransient<JsonService>();
builder.Services.AddSingleton<DataInicialService>();
builder.Services.AddSingleton<ListaSelectorTipoPolizaService>();
builder.Services.AddSingleton<ListaSelectorPolizaCoberturaService>();
builder.Services.AddSingleton<ListaSelectorPolizaEstadoService>();
builder.Services.AddTransient<CrearPolizaService>();
builder.Services.AddTransient<ObtenerPolizaService>();
builder.Services.AddTransient<ActualizarPolizaService>();
builder.Services.AddTransient<EliminarPolizaService>();
builder.Services.AddTransient<ObtenerPorIdService>();
builder.Services.AddDbContext<PolizaDbContext>(options =>
{
    SqlConnection sqlConnection = new SqlConnection();
    sqlConnection.ConnectionString = Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING") ?? "Server=localhost;Database=PolizaDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
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

using (var dbContext = app.Services.GetRequiredService<DbContextFactoryService>().CreateDbContext<PolizaDbContext>())
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
    await service.IniciarInsersiones();
});

app.Run();
