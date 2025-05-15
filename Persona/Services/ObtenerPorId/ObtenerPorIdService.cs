using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persona.Entities;
using Persona.Models.ObtenerPorId;
using System.Net;
using System.Reflection;

namespace Persona.Services.ObtenerPorId
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

                bool cedulaEsNula = httpContext.Request.RouteValues.ContainsKey("cedulaAsegurado");
                if (!cedulaEsNula)
                {
                    _logger.LogError(string.Format("{0} - El parámetro identificador de la persona no está en la solicitud", httpContext.TraceIdentifier));
                    throw new BPSegurosException((int)HttpStatusCode.BadRequest, "El parámetro identificador de la persona es requerido y no fue proporcionado en la solicitud.");
                }

                string cedula = httpContext.Request.RouteValues["cedulaAsegurado"]!.ToString()!;
                _logger.LogInformation(string.Format("{0} - El parámetro cedulaAsegurado tiene el siguiente valor: '{1}'", httpContext.TraceIdentifier, cedula));

                using (var dbContext = _dbContextFactoryService.CreateDbContext<PersonaDbContext>())
                {
                    var personaEntity = await dbContext.Persona.FirstOrDefaultAsync(x => x.CedulaAsegurado == cedula);
                    if (personaEntity is null)
                    {
                        _logger.LogError(string.Format("{0} - La persona no existe con la cédula '{1}'", traceId, cedula));
                        throw new BPSegurosException((int)HttpStatusCode.NotFound, "La persona no ha sido encontrada.");
                    }

                    ObtenerPorIdResponseModel responseModel = new()
                    {
                        CedulaAsegurado = personaEntity.CedulaAsegurado,
                        FechaNacimiento = personaEntity.FechaNacimiento,
                        IdTipoPersona = personaEntity.IdTipoPersona,
                        Nombre = personaEntity.Nombre,
                        PrimerApellido = personaEntity.PrimerApellido,
                        SegundoApellido = personaEntity.SegundoApellido,
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
                BPSegurosException argosEx = new BPSegurosException((int)HttpStatusCode.InternalServerError, "Ha ocurrido un error al obtener el detalle de la persona.", ex);
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