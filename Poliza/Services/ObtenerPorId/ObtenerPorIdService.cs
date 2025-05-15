using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Poliza.Entities;
using System.Net;
using System.Reflection;

namespace Poliza.Services.ObtenerPorId
{
    public class ObtenerPorIdService
    {
        private readonly ILogger<ObtenerPorIdService> _logger;
        private readonly DbContextFactoryService _dbContextFactoryService;

        public ObtenerPorIdService(ILogger<ObtenerPorIdService> logger, DbContextFactoryService dbContextFactoryService)
        {
            _logger = logger;
            _dbContextFactoryService = dbContextFactoryService;
        }

        public async Task<IActionResult> ObtenerPorId(HttpContext httpContext)
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

                using (var dbContext = _dbContextFactoryService.CreateDbContext<PolizaDbContext>())
                {
                    var personaEntity = await dbContext.Poliza.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
                    if (personaEntity is null)
                    {
                        _logger.LogError(string.Format("{0} - La póliza no existe con el id '{1}'", traceId, id));
                        throw new BPSegurosException((int)HttpStatusCode.NotFound, "La póliza no ha sido encontrada.");
                    }

                    Poliza.Models.ObtenerPorId.ObtenerPorIdResponseModel responseModel = new()
                    {
                        CedulaAsegurado = personaEntity.CedulaAsegurado,
                        FechaVencimiento = personaEntity.FechaVencimiento,
                        Aseguradora = personaEntity.Aseguradora,
                        Id = personaEntity.Id.ToString(),
                        FechaEmision = personaEntity.FechaEmision,
                        FechaInclusion = personaEntity.FechaInclusion,
                        IdCobertura = personaEntity.IdCobertura.ToString(),
                        IdPolizaEstado = personaEntity.IdPolizaEstado.ToString(),
                        IdTipoPoliza = personaEntity.IdTipoPoliza.ToString(),
                        MontoAsegurado = personaEntity.MontoAsegurado,
                        NumeroPoliza = personaEntity.NumeroPoliza,
                        Periodo = personaEntity.Periodo,
                        Prima = personaEntity.Prima,
                    };

                    IActionResult actionResult = new ObjectResult(responseModel);

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
                BPSegurosException argosEx = new BPSegurosException((int)HttpStatusCode.InternalServerError, "Ha ocurrido un error al obtener el detalle de la póliza.", ex);
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
