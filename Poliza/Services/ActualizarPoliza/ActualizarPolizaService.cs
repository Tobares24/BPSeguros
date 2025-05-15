using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Poliza.Entities;
using Poliza.Models.ActualizarPoliza;
using System.Net;
using System.Reflection;

namespace Poliza.Services.ActualizarPoliza
{
    public class ActualizarPolizaService
    {
        private readonly ILogger<ActualizarPolizaService> _logger;
        private readonly DbContextFactoryService _dbContextFactoryService;
        private readonly RequestModelValidationService _validationService;
        private readonly JsonService _jsonService;

        public ActualizarPolizaService(ILogger<ActualizarPolizaService> logger, DbContextFactoryService dbContextFactoryService, JsonService jsonService,
            RequestModelValidationService validationService)
        {
            _logger = logger;
            _dbContextFactoryService = dbContextFactoryService;
            _jsonService = jsonService;
            _validationService = validationService;
        }

        public async Task<IActionResult> Actualizar(HttpContext httpContext)
        {
            try
            {
                _logger.LogInformation(string.Format("{0} - Iniciando ejecución del método {1}", httpContext.TraceIdentifier, MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name));

                string traceId = httpContext.TraceIdentifier;

                bool idEsNulo = httpContext.Request.RouteValues.ContainsKey("id");
                if (!idEsNulo)
                {
                    _logger.LogError(string.Format("{0} - El parámetro identificador de la póliza no está en la solicitud", httpContext.TraceIdentifier));
                    throw new BPSegurosException((int)HttpStatusCode.BadRequest, "El parámetro identificador de la póliza es requerido y no fue proporcionado en la solicitud.");
                }

                string id = httpContext.Request.RouteValues["id"]!.ToString()!;
                _logger.LogInformation(string.Format("{0} - El parámetro id tiene el siguiente valor: '{1}'", httpContext.TraceIdentifier, id));

                ActualizarPolizaRequestModel requestModel = await _jsonService.RequestToObjectAsync<ActualizarPolizaRequestModel>(httpContext);

                _validationService.Validate(requestModel, traceId);

                using (var dbContext = _dbContextFactoryService.CreateDbContext<PolizaDbContext>())
                {
                    var existePoliza = await dbContext.Poliza.AnyAsync(x => !x.EstaEliminado && x.Id == Guid.Parse(id));
                    if (!existePoliza)
                    {
                        _logger.LogError(string.Format("{0} - Ya existe un registro con el numero de póliza '{1}'", traceId, requestModel.NumeroPoliza));
                        throw new BPSegurosException((int)HttpStatusCode.Conflict, "El número de póliza ya existe.");
                    }

                    var existeTipoPoliza = await dbContext.TipoPoliza.AnyAsync(x => !x.EstaEliminado && x.Id == Guid.Parse(requestModel.IdTipoPoliza!));
                    if (!existeTipoPoliza)
                    {
                        _logger.LogError(string.Format("{0} - El tipo de póliza no ha sido encontrado con el identificador '{1}'", traceId, requestModel.IdTipoPoliza));
                        throw new BPSegurosException((int)HttpStatusCode.NoContent, "El tipo de póliza no ha sido encontrado.");
                    }

                    if (!string.IsNullOrEmpty(requestModel.IdCobertura))
                    {
                        var existeCobertura = await dbContext.PolizaCobertura.AnyAsync(x => !x.EstaEliminado && x.Id == Guid.Parse(requestModel.IdCobertura!));
                        if (!existeCobertura)
                        {
                            _logger.LogError(string.Format("{0} - La cobertura no ha sido encontrada con el identificador '{1}'", traceId, requestModel.IdCobertura));
                            throw new BPSegurosException((int)HttpStatusCode.NoContent, "La cobertura no ha sido encontrada.");
                        }
                    }

                    await dbContext.Poliza
                        .Where(x => x.Id == Guid.Parse(id))
                        .ExecuteUpdateAsync(set => set
                            .SetProperty(y => y.IdCobertura,
                                !string.IsNullOrEmpty(requestModel.IdCobertura)
                                ? Guid.Parse(requestModel.IdCobertura!)
                                : null)
                            .SetProperty(y => y.Periodo, requestModel.Periodo)
                            .SetProperty(y => y.Aseguradora, requestModel.Aseguradora)
                            .SetProperty(y => y.FechaEmision, requestModel.FechaEmision)
                            .SetProperty(y => y.FechaInclusion, requestModel.FechaInclusion)
                            .SetProperty(y => y.FechaVencimiento, requestModel.FechaVencimiento)
                            .SetProperty(y => y.MontoAsegurado, requestModel.MontoAsegurado)
                            .SetProperty(y => y.NumeroPoliza, requestModel.NumeroPoliza)
                            .SetProperty(y => y.Prima, requestModel.Prima)
                            .SetProperty(y => y.IdTipoPoliza, Guid.Parse(requestModel.IdTipoPoliza!))
                            .SetProperty(y => y.IdPolizaEstado, Guid.Parse(requestModel.IdPolizaEstado!))
                        );

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
                BPSegurosException argosEx = new BPSegurosException((int)HttpStatusCode.InternalServerError, "Ha ocurrido un error al actualizar la póliza.", ex);
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
