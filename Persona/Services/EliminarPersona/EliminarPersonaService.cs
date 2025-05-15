using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persona.Entities;
using System.Net;
using System.Reflection;

namespace Persona.Services.EliminarPersona
{
    public class EliminarPersonaService
    {
        private readonly ILogger<EliminarPersonaService> _logger;
        private readonly DbContextFactoryService _dbContextFactoryService;

        public EliminarPersonaService(ILogger<EliminarPersonaService> logger, DbContextFactoryService dbContextFactoryService)
        {
            _logger = logger;
            _dbContextFactoryService = dbContextFactoryService;
        }

        public async Task<IActionResult> Eliminar(HttpContext httpContext)
        {
            try
            {
                _logger.LogInformation(string.Format("{0} - Iniciando ejecución del método {1}", httpContext.TraceIdentifier, MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name));

                string traceId = httpContext.TraceIdentifier;

                bool cedulaEsNula = httpContext.Request.RouteValues.ContainsKey("cedulaAsegurado");
                if (!cedulaEsNula)
                {
                    _logger.LogError(string.Format("{0} - El parámetro cédula de la persona no está en la solicitud", httpContext.TraceIdentifier));
                    throw new BPSegurosException((int)HttpStatusCode.BadRequest, "El parámetro cédula de la persona es requerido y no fue proporcionado en la solicitud.");
                }

                string cedula = httpContext.Request.RouteValues["cedulaAsegurado"]!.ToString()!;
                _logger.LogInformation(string.Format("{0} - El parámetro cedulaAsegurado tiene el siguiente valor: '{1}'", httpContext.TraceIdentifier, cedula));

                using (var dbContext = _dbContextFactoryService.CreateDbContext<PersonaDbContext>())
                {
                    bool existePersona = await dbContext.Persona.AnyAsync(x => x.CedulaAsegurado == cedula);
                    if (!existePersona)
                    {
                        _logger.LogError(string.Format("{0} - la persona no existe con la cédula '{1}'", traceId, cedula));
                        throw new BPSegurosException((int)HttpStatusCode.NotFound, "La persona no ha sido encontrada.");
                    }

                    await dbContext.Persona.Where(x => !x.EstaEliminado && x.CedulaAsegurado == cedula)
                        .ExecuteUpdateAsync(x => x.SetProperty(y => y.EstaEliminado, true));

                    using (var transaction = await dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
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
                }

                IActionResult actionResult = new StatusCodeResult((int)HttpStatusCode.NoContent);

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
                BPSegurosException argosEx = new BPSegurosException((int)HttpStatusCode.InternalServerError, "Ha ocurrido un error al eliminar la persona.", ex);
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