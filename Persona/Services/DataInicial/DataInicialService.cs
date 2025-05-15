using Common.Entities;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Persona.Entities;
using System.Reflection;

namespace Persona.Services.DataInicial
{
    public class DataInicialService
    {
        private readonly ILogger<DataInicialService> _logger;
        private readonly DbContextFactoryService _dbContextFactoryService;

        public DataInicialService(ILogger<DataInicialService> logger, DbContextFactoryService dbContextFactoryService)
        {
            _logger = logger;
            _dbContextFactoryService = dbContextFactoryService;
        }

        public async Task CrearTipoPersonas()
        {
            try
            {
                _logger.LogInformation("Inicio de invocación del método {0}", $"{MethodBase.GetCurrentMethod()!.ReflectedType!.FullName}.{MethodBase.GetCurrentMethod()!.Name}");

                using (var dbContext = _dbContextFactoryService.CreateDbContext<PersonaDbContext>())
                {
                    var tipoFisica = "Física";
                    var tipoJuridica = "Jurídica";

                    bool existeFisica = await dbContext.TipoPersona
                        .AnyAsync(x => x.TipoPersona!.ToLower() == tipoFisica.ToLower());

                    bool existeJuridica = await dbContext.TipoPersona
                        .AnyAsync(x => x.TipoPersona!.ToLower() == tipoJuridica.ToLower());

                    var nuevosTipos = new List<TipoPersonaEntity>();

                    if (!existeFisica)
                    {
                        nuevosTipos.Add(new TipoPersonaEntity
                        {
                            TipoPersona = tipoFisica,
                            EstaEliminado = false
                        });
                    }

                    if (!existeJuridica)
                    {
                        nuevosTipos.Add(new TipoPersonaEntity
                        {
                            TipoPersona = tipoJuridica,
                            EstaEliminado = false
                        });
                    }

                    if (nuevosTipos.Any())
                    {
                        await dbContext.TipoPersona.AddRangeAsync(nuevosTipos);

                        using (var transaction = await dbContext.Database.BeginTransactionAsync())
                        {
                            try
                            {
                                await dbContext.SaveChangesAsync();
                                await transaction.CommitAsync();
                            }
                            catch (Exception ex)
                            {
                                await transaction.RollbackAsync();
                                _logger.LogError("Error al guardar tipos de persona: {0}", ex.ToString());
                                throw;
                            }
                        }
                    }
                    else
                    {
                        _logger.LogInformation("Los tipos de persona ya existen. No se insertó ningún dato.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Excepción en CrearTipoPersonas: {0}", ex.ToString());
            }
            finally
            {
                _logger.LogInformation("Fin de invocación del método {0}", $"{MethodBase.GetCurrentMethod()!.ReflectedType!.FullName}.{MethodBase.GetCurrentMethod()!.Name}");
            }
        }
    }
}