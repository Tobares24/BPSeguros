using Common.Entities;
using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Poliza.Entities;
using Poliza.Models.CrearPoliza;
using System.Net;
using System.Reflection;

namespace Poliza.Services
{
    public class CrearPolizaService
    {
        private readonly ILogger<CrearPolizaService> _logger;
        private readonly DbContextFactoryService _dbContextFactoryService;
        private readonly RequestModelValidationService _validationService;
        private readonly JsonService _jsonService;
        private readonly InternalService _internalService;

        public CrearPolizaService(ILogger<CrearPolizaService> logger, DbContextFactoryService dbContextFactoryService, JsonService jsonService,
            RequestModelValidationService validationService, InternalService internalService)
        {
            _logger = logger;
            _dbContextFactoryService = dbContextFactoryService;
            _jsonService = jsonService;
            _validationService = validationService;
            _internalService=internalService;
        }

        public async Task<IActionResult> CrearPoliza(HttpContext httpContext)
        {
            try
            {
                _logger.LogInformation(string.Format("{0} - Iniciando ejecución del método {1}", httpContext.TraceIdentifier, MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name));

                string traceId = httpContext.TraceIdentifier;

                CrearPolizaRequestModel requestModel = await _jsonService.RequestToObjectAsync<CrearPolizaRequestModel>(httpContext);

                _validationService.Validate(requestModel, traceId);

                try
                {
                    var personaResponseJson = await _internalService.GetEntityById<IDictionary<string, object>>(httpContext.TraceIdentifier,
                       "api/personas", requestModel.CedulaAsegurado!, "persona");

                    ObtenerPorIdResponseModel personaObtenida = _jsonService.ConvertToObject<ObtenerPorIdResponseModel>(personaResponseJson!["Persona"].ToString()!);
                }
                catch (BPSegurosException ex)
                {
                    if (ex.StatusCode == 404)
                    {
                        ex.StatusCode = 400;
                    }
                    throw;
                }

                using (var dbContext = _dbContextFactoryService.CreateDbContext<PolizaDbContext>())
                {
                    var existePoliza = await dbContext.Poliza.AnyAsync(x => !x.EstaEliminado && x.NumeroPoliza == requestModel.NumeroPoliza);
                    if (existePoliza)
                    {
                        _logger.LogError(string.Format("{0} - Ya existe un registro con el numero de póliza '{1}'", traceId, requestModel.NumeroPoliza));
                        throw new BPSegurosException((int)HttpStatusCode.Conflict, "El número de póliza ya existe.");
                    }

                    var existeTipoPoliza = await dbContext.TipoPoliza.AnyAsync(x => !x.EstaEliminado && x.Id == requestModel.IdTipoPoliza);
                    if (!existeTipoPoliza)
                    {
                        _logger.LogError(string.Format("{0} - El tipo de póliza no ha sido encontrado con el identificador '{1}'", traceId, requestModel.IdTipoPoliza));
                        throw new BPSegurosException((int)HttpStatusCode.NoContent, "El tipo de póliza no ha sido encontrado.");
                    }

                    var existeCobertura = await dbContext.PolizaCobertura.AnyAsync(x => !x.EstaEliminado && requestModel.IdCobertura != null && x.Id == requestModel.IdCobertura);
                    if (!existeCobertura)
                    {
                        _logger.LogError(string.Format("{0} - La cobertura no ha sido encontrada con el identificador '{1}'", traceId, requestModel.IdCobertura));
                        throw new BPSegurosException((int)HttpStatusCode.NoContent, "La cobertura no ha sido encontrada.");
                    }

                    PolizaEntity polizaEntity = new()
                    {
                        NumeroPoliza = requestModel.NumeroPoliza,
                        Aseguradora = requestModel.Aseguradora,
                        FechaEmision = requestModel.FechaEmision,
                        FechaInclusion = requestModel.FechaInclusion,
                        FechaVencimiento = requestModel.FechaVencimiento,
                        IdCobertura  = requestModel.IdCobertura,
                        IdPolizaEstado = requestModel.IdPolizaEstado,
                        IdTipoPoliza = requestModel.IdTipoPoliza,
                        CedulaAsegurado = requestModel.CedulaAsegurado,
                        MontoAsegurado = requestModel.MontoAsegurado,
                        Prima = requestModel.Prima,
                        Periodo = requestModel.Periodo,
                        EstaEliminado = false,
                    };

                    await dbContext.Poliza.AddAsync(polizaEntity);

                    using (var transaction = await dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            await dbContext.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                        catch (BPSegurosException ex)
                        {
                            _logger.LogError(string.Format("{0} - BPSegurosException: {1}", traceId, ex.ToString()));
                            await transaction.RollbackAsync();
                            throw;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(string.Format("{0} - Exception: {1}", traceId, ex.ToString()));
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }

                    IActionResult actionResult = new StatusCodeResult((int)HttpStatusCode.OK);

                    return actionResult;
                }
            }
            catch (BPSegurosException ex)
            {
                _logger.LogError(string.Format("{0} - BPSegurosException: {1}", httpContext.TraceIdentifier, ex.ToString()));
                return new ObjectResult(new ErrorResponseModel
                {
                    Message = ex.Message,
                    StatusCode = ex.StatusCode,
                    TraceId = httpContext.TraceIdentifier,
                    FieldName = ex.FieldName
                })
                {
                    StatusCode = ex.StatusCode
                };
            }
            catch (Exception ex)
            {
                BPSegurosException argosEx = new BPSegurosException((int)HttpStatusCode.InternalServerError, "Ha ocurrido un error al crear la póliza.", ex);
                _logger.LogError(string.Format("{0} - Exception: {1}", httpContext.TraceIdentifier, argosEx.ToString()));
                return new ObjectResult(new ErrorResponseModel
                {
                    Message = argosEx.Message,
                    StatusCode = argosEx.StatusCode,
                    TraceId = httpContext.TraceIdentifier
                })
                {
                    StatusCode = argosEx.StatusCode
                };
            }
            finally
            {
                _logger.LogInformation(string.Format("{0} - Finalizando ejecución del método {1}", httpContext.TraceIdentifier, MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name));
            }
        }
    }
}