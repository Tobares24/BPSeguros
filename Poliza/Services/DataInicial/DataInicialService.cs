using Common.Entities;
using Common.Services;
using Poliza.Entities;
using System.Reflection;

namespace Poliza.Services.DataInicial
{
    public class DataInicialService
    {
        private readonly ILogger<DataInicialService> _logger;
        private readonly DbContextFactoryService _dbContextFactoryService;

        private static readonly List<string> _tiposPredefinidos = new()
        {
            "Seguro de Vida",
            "Seguro de Salud",
            "Seguro de Automóvil",
            "Seguro de Hogar",
            "Seguro de Viaje",
            "Responsabilidad Civil",
            "Seguro Empresarial",
            "Incapacidad Temporal o Permanente"
        };

        private static readonly List<string> _coberturasPredefinidas = new()
        {
            "Muerte Accidental",
            "Incapacidad Total y Permanente",
            "Gastos Médicos",
            "Hospitalización",
            "Daños Materiales",
            "Robo o Hurto",
            "Responsabilidad Civil",
            "Asistencia en Viaje",
            "Cobertura contra Incendio",
            "Cobertura por Terremoto"
        };

        private static readonly List<string> _estadosPredefinidos = new()
        {
            "Activa",
            "Pendiente de Pago",
            "Vencida",
            "Cancelada",
            "Suspendida Temporalmente",
            "Finalizada",
            "En Revisión"
        };

        public DataInicialService(ILogger<DataInicialService> logger, DbContextFactoryService dbContextFactoryService)
        {
            _logger = logger;
            _dbContextFactoryService = dbContextFactoryService;
        }

        public async Task IniciarInsersiones()
        {
            try
            {
                Task crearTipoPoliza = CrearTipoPoliza();
                Task crearCoberturas = CrearCoberturas();
                Task crearEstados = CrearEstadosPoliza();

                await Task.WhenAll(crearTipoPoliza, crearCoberturas, crearEstados);
            }
            catch (Exception ex)
            {
                _logger.LogError("Excepción en CrearTipoPoliza: {0}", ex.ToString());
            }
            finally
            {
                _logger.LogInformation("Fin de invocación del método {0}", $"{MethodBase.GetCurrentMethod()!.ReflectedType!.FullName}.{MethodBase.GetCurrentMethod()!.Name}");
            }
        }

        public async Task CrearTipoPoliza()
        {
            try
            {
                _logger.LogInformation("Inicio de invocación del método {0}", $"{MethodBase.GetCurrentMethod()!.ReflectedType!.FullName}.{MethodBase.GetCurrentMethod()!.Name}");

                using (var dbContext = _dbContextFactoryService.CreateDbContext<PolizaDbContext>())
                {
                    var existentes = dbContext.Set<TipoPolizaEntity>()
                        .Where(tp => _tiposPredefinidos.Contains(tp.Descripcion!) && !tp.EstaEliminado)
                        .Select(tp => tp.Descripcion!)
                        .ToHashSet();

                    var nuevos = _tiposPredefinidos
                        .Where(desc => !existentes.Contains(desc))
                        .Select(desc => new TipoPolizaEntity
                        {
                            Descripcion = desc,
                            EstaEliminado = false
                        }).ToList();

                    if (nuevos.Any())
                    {
                        dbContext.Set<TipoPolizaEntity>().AddRange(nuevos);

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
                                _logger.LogError("Error al guardar los tipos de pólizas: {0}", ex.ToString());
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Excepción en CrearTipoPoliza: {0}", ex.ToString());
                throw;
            }
            finally
            {
                _logger.LogInformation("Fin de invocación del método {0}", $"{MethodBase.GetCurrentMethod()!.ReflectedType!.FullName}.{MethodBase.GetCurrentMethod()!.Name}");
            }
        }

        public async Task CrearCoberturas()
        {
            try
            {
                _logger.LogInformation("Inicio de invocación del método {0}", $"{MethodBase.GetCurrentMethod()!.ReflectedType!.FullName}.{MethodBase.GetCurrentMethod()!.Name}");

                using (var dbContext = _dbContextFactoryService.CreateDbContext<PolizaDbContext>())
                {
                    var existentes = dbContext.Set<PolizaCoberturaEntity>()
                        .Where(c => _coberturasPredefinidas.Contains(c.Descripcion!) && !c.EstaEliminado)
                        .Select(c => c.Descripcion!)
                        .ToHashSet();

                    var nuevos = _coberturasPredefinidas
                        .Where(desc => !existentes.Contains(desc))
                        .Select(cp => new PolizaCoberturaEntity
                        {
                            Descripcion = cp,
                            EstaEliminado = false
                        }).ToList();

                    if (nuevos.Any())
                    {
                        dbContext.Set<PolizaCoberturaEntity>().AddRange(nuevos);

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
                                _logger.LogError("Error al guardar las coberturas: {0}", ex.ToString());
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Excepción en CrearCoberturas: {0}", ex.ToString());
                throw;
            }
            finally
            {
                _logger.LogInformation("Fin de invocación del método {0}", $"{MethodBase.GetCurrentMethod()!.ReflectedType!.FullName}.{MethodBase.GetCurrentMethod()!.Name}");
            }
        }

        public async Task CrearEstadosPoliza()
        {
            try
            {
                _logger.LogInformation("Inicio de invocación del método {0}", $"{MethodBase.GetCurrentMethod()!.ReflectedType!.FullName}.{MethodBase.GetCurrentMethod()!.Name}");

                using (var dbContext = _dbContextFactoryService.CreateDbContext<PolizaDbContext>())
                {
                    var existentes = dbContext.Set<PolizaEstadoEntity>()
                        .Where(e => _estadosPredefinidos.Contains(e.Descripcion!) && !e.EstaEliminado)
                        .Select(e => e.Descripcion!)
                        .ToHashSet();

                    var nuevos = _estadosPredefinidos
                        .Where(desc => !existentes.Contains(desc))
                        .Select(desc => new PolizaEstadoEntity
                        {
                            Descripcion = desc,
                            EstaEliminado = false
                        }).ToList();

                    if (nuevos.Any())
                    {
                        dbContext.Set<PolizaEstadoEntity>().AddRange(nuevos);

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
                                _logger.LogError("Error al guardar los estados de póliza: {0}", ex.ToString());
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Excepción en CrearEstadosPoliza: {0}", ex.ToString());
                throw;
            }
            finally
            {
                _logger.LogInformation("Fin de invocación del método {0}", $"{MethodBase.GetCurrentMethod()!.ReflectedType!.FullName}.{MethodBase.GetCurrentMethod()!.Name}");
            }
        }
    }
}
