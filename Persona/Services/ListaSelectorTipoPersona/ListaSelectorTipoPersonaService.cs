using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persona.Entities;
using Persona.Models.ListaSelectorTipoPersona;
using System.Net;
using System.Reflection;

namespace Persona.Services.ListaSelectorTipoPersona
{
    public class ListaSelectorTipoPersonaService
    {
        private readonly ILogger<ListaSelectorTipoPersonaService> _logger;
        private readonly DbContextFactoryService _dbContextFactoryService;

        public ListaSelectorTipoPersonaService(ILogger<ListaSelectorTipoPersonaService> logger, DbContextFactoryService dbContextFactoryService)
        {
            _logger = logger;
            _dbContextFactoryService = dbContextFactoryService;
        }

        public async Task<IActionResult> ListaSelectorTipoPersona(HttpContext httpContext)
        {
            try
            {
                _logger.LogInformation(string.Format("{0} - Iniciando ejecución del método {1}", httpContext.TraceIdentifier, MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name));

                string traceId = httpContext.TraceIdentifier;

                string? filtro = httpContext.Request.Query["filtro"];

                List<ListaSelectorTipoPersonaResponseModel> registros = new();

                using (var dbContext = _dbContextFactoryService.CreateDbContext<PersonaDbContext>())
                {
                    var query = dbContext.TipoPersona.Where(x => !x.EstaEliminado);

                    query = !string.IsNullOrEmpty(filtro) ? query.Where(x => (x.TipoPersona != null && x.TipoPersona.StartsWith(filtro))) : query;

                    registros = await query
                         .Take(10)
                         .Select(x => new ListaSelectorTipoPersonaResponseModel
                         {
                             Id = x.Id,
                             TipoPersona = x.TipoPersona,
                         }).ToListAsync();
                }

                IActionResult actionResult = new ObjectResult(registros);

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
                BPSegurosException argosEx = new BPSegurosException((int)HttpStatusCode.InternalServerError, "Ha ocurrido un error al obtener los tipos de personas del selector.", ex);
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
