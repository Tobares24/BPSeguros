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

                ObtenerPersonaResponseModel responseModel = new()
                {
                    Registros = new(),
                };

                List<PersonaEntity> registros = new();

                string? nombre = httpContext.Request.RouteValues["nombre"]?.ToString();
                string? cedulaAsegurado = httpContext.Request.RouteValues["cedulaAsegurado"]?.ToString();
                string? primerApellido = httpContext.Request.RouteValues["primerApellido"]?.ToString();
                string? segundoApellido = httpContext.Request.RouteValues["segundoApellido"]?.ToString();
                string? idTipoPersona = httpContext.Request.RouteValues["idTipoPersona"]?.ToString();
                string? paginaActualParams = httpContext.Request.RouteValues["paginaActual"]?.ToString();
                string? cantidadRegistrosParams = httpContext.Request.RouteValues["cantidadRegistros"]?.ToString();

                int paginaActual = int.Parse(paginaActualParams ?? "1");
                int cantidadRegistros = int.Parse(cantidadRegistrosParams ?? "10");

                using (var dbContext = _dbContextFactoryService.CreateDbContext<PersonaDbContext>())
                {
                    var query = dbContext.Persona.Include(x => x.TipoPersona).Where(x => !x.EstaEliminado && x.TipoPersona != null);

                    query = !string.IsNullOrEmpty(nombre) ? query.Where(x => x.Nombre!.ToLower().StartsWith(nombre)) : query;
                    query = !string.IsNullOrEmpty(cedulaAsegurado) ? query.Where(x => x.CedulaAsegurado!.ToLower().StartsWith(cedulaAsegurado)) : query;
                    query = !string.IsNullOrEmpty(primerApellido) ? query.Where(x => x.PrimerApellido!.ToLower().StartsWith(primerApellido)) : query;
                    query = !string.IsNullOrEmpty(segundoApellido) ? query.Where(x => x.SegundoApellido!.ToLower().StartsWith(segundoApellido)) : query;
                    query = idTipoPersona is not null ? query.Where(x => x.IdTipoPersona == Guid.Parse(idTipoPersona)) : query;

                    responseModel.CantidadRegistrosPaginas = await query.CountAsync();
                    responseModel.CantidadPaginas = (int)Math.Ceiling((decimal)responseModel.CantidadRegistrosPaginas / (decimal)cantidadRegistros);

                    registros = await query
                         .Skip((paginaActual - 1) * cantidadRegistros)
                         .Take(cantidadRegistros)
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

                    responseModel.PaginaActual = paginaActual;
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
