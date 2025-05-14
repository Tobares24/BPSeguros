using Common.Entities;
using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persona.Entities;
using Persona.Models.ObtenerPersona;
using System.Net;
using System.Reflection;

namespace Persona.Services.ObtenerPersona
{
    public class ObtenerPersonaService
    {
        private readonly ILogger<ObtenerPersonaService> _logger;
        private readonly DbContextFactoryService _dbContextFactoryService;
        private readonly JsonService _jsonService;

        public ObtenerPersonaService(ILogger<ObtenerPersonaService> logger, DbContextFactoryService dbContextFactoryService, JsonService jsonService)
        {
            _logger = logger;
            _dbContextFactoryService = dbContextFactoryService;
            _jsonService = jsonService;
        }

        public async Task<IActionResult> Obtener(HttpContext httpContext)
        {
            try
            {
                _logger.LogInformation(string.Format("{0} - Iniciando ejecución del método {1}", httpContext.TraceIdentifier, MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name));

                string traceId = httpContext.TraceIdentifier;

                ObtenerPersonaRequestModel? requestModel = await _jsonService.RequestToObjectAsync<ObtenerPersonaRequestModel>(httpContext);

                ObtenerPersonaResponseModel responseModel = new()
                {
                    Registros = new(),
                };

                List<PersonaEntity> registros = new();

                using (var dbContext = _dbContextFactoryService.CreateDbContext<PersonaDbContext>())
                {
                    var query = dbContext.Persona.Include(x => x.TipoPersona).Where(x => !x.EstaEliminado && x.TipoPersona != null);

                    query = !string.IsNullOrEmpty(requestModel.Nombre) ? query.Where(x => x.Nombre!.ToLower().StartsWith(requestModel.Nombre)) : query;
                    query = !string.IsNullOrEmpty(requestModel.CedulaAsegurado) ? query.Where(x => x.CedulaAsegurado!.ToLower().StartsWith(requestModel.CedulaAsegurado)) : query;
                    query = !string.IsNullOrEmpty(requestModel.PrimerApellido) ? query.Where(x => x.PrimerApellido!.ToLower().StartsWith(requestModel.PrimerApellido)) : query;
                    query = !string.IsNullOrEmpty(requestModel.SegundoApellido) ? query.Where(x => x.SegundoApellido!.ToLower().StartsWith(requestModel.SegundoApellido)) : query;
                    query = requestModel.IdTipoPersona is not null ? query.Where(x => x.IdTipoPersona == requestModel.IdTipoPersona) : query;

                    responseModel.CantidadRegistrosPaginas = await query.CountAsync();
                    responseModel.CantidadPaginas = (int)Math.Ceiling((decimal)responseModel.CantidadRegistrosPaginas / (decimal)requestModel.CantidadRegistros);

                    registros = await query
                         .Skip((requestModel.PaginaActual - 1) * requestModel.CantidadRegistros)
                         .Take(requestModel.CantidadRegistros)
                         .ToListAsync();
                }

                if (registros.Any())
                {
                    responseModel.Registros = registros.Select(persona => new PersonaModel
                    {
                        CedulaAsegurado = persona.CedulaAsegurado,
                        FechaNacimiento = persona.FechaNacimiento,
                        Nombre = persona.Nombre,
                        PrimerApellido = persona.PrimerApellido,
                        SegundoApellido = persona.SegundoApellido,
                        TipoPersona = persona.TipoPersona?.TipoPersona,
                    }).ToList();

                    responseModel.PaginaActual = requestModel.PaginaActual;
                    responseModel.CantidadRegistros = responseModel.Registros.Count();
                }

                IActionResult actionResult = new ObjectResult(responseModel);

                return actionResult;
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
                BPSegurosException argosEx = new BPSegurosException((int)HttpStatusCode.InternalServerError, "Ha ocurrido un error al obtener las personas.", ex);
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
