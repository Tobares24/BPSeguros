using Common.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Poliza.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddSingleton<DbContextFactoryService>();
builder.Services.AddDbContext<PolizaDbContext>(options =>
{
    SqlConnection sqlConnection = new SqlConnection();
    sqlConnection.ConnectionString = Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING") ?? "Server=localhost;Database=PolizaDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
    options.UseSqlServer(sqlConnection);
});

var app = builder.Build();

using (var dbContext = app.Services.GetRequiredService<DbContextFactoryService>().CreateDbContext<PolizaDbContext>())
{
    await dbContext.Database.EnsureCreatedAsync();
    await dbContext.Database.MigrateAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
