using Common.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Poliza.Entities;
using Poliza.Services.DataInicial;
using Poliza.Services.ListaSelectorPolizaCobertura;
using Poliza.Services.ListaSelectorPolizaEstado;
using Poliza.Services.ListaSelectorPolizaPeriodo;
using Poliza.Services.ListaSelectorTipoPoliza;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddSingleton<DbContextFactoryService>();
builder.Services.AddSingleton<DataInicialService>();
builder.Services.AddSingleton<ListaSelectorTipoPolizaService>();
builder.Services.AddSingleton<ListaSelectorPolizaCoberturaService>();
builder.Services.AddSingleton<ListaSelectorPolizaEstadoService>();
builder.Services.AddSingleton<ListaSelectorPolizaPeriodoService>();
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

var app = builder.Build();

using (var dbContext = app.Services.GetRequiredService<DbContextFactoryService>().CreateDbContext<PolizaDbContext>())
{
    await dbContext.Database.EnsureCreatedAsync();
    await dbContext.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseCors("*");
app.UseAuthorization();
app.MapControllers();

_ = Task.Run(async () =>
{
    DataInicialService service = app.Services.GetService<DataInicialService>()!;
    await service.IniciarInsersiones();
});

app.Run();
