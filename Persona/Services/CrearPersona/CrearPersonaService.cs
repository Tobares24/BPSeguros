using Common.Entities;
using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persona.Entities;
using Persona.Models.CrearPersona;
using System.Net;
using System.Reflection;

namespace Persona.Services.CrearPersona
{
    public class CrearPersonaService
    {
        private readonly ILogger<CrearPersonaService> _logger;
        private readonly DbContextFactoryService _dbContextFactoryService;
        private readonly RequestModelValidationService _validationService;
        private readonly JsonService _jsonService;

        public CrearPersonaService(ILogger<CrearPersonaService> logger, DbContextFactoryService dbContextFactoryService, JsonService jsonService,
            RequestModelValidationService validationService)
        {
            _logger = logger;
            _dbContextFactoryService = dbContextFactoryService;
            _jsonService = jsonService;
            _validationService = validationService;
        }

        public async Task<IActionResult> Crear(HttpContext httpContext)
        {
            try
            {
                _logger.LogInformation(string.Format("{0} - Iniciando ejecución del método {1}", httpContext.TraceIdentifier, MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name));

                string traceId = httpContext.TraceIdentifier;

                CrearPersonaRequestModel requestModel = await _jsonService.RequestToObjectAsync<CrearPersonaRequestModel>(httpContext);

                _validationService.Validate(requestModel, traceId);

                using (var dbContext = _dbContextFactoryService.CreateDbContext<PersonaDbContext>())
                {
                    var existeTipoPersona = await dbContext.TipoPersona.AnyAsync(x => !x.EstaEliminado && x.Id == requestModel.IdTipoPersona);
                    if (!existeTipoPersona)
                    {
                        _logger.LogError(string.Format("{0} - El tipo de persona no ha sido encontrado con el identificador '{1}'", traceId, requestModel.IdTipoPersona));
                        throw new BPSegurosException((int)HttpStatusCode.NoContent, "El tipo de persona no ha sido encontrado.");
                    }

                    var existePersona = await dbContext.Persona.AnyAsync(x => !x.EstaEliminado && x.CedulaAsegurado == requestModel.CedulaAsegurado);
                    if (existePersona)
                    {
                        _logger.LogError(string.Format("{0} - Ya existe un registro con la cédula '{1}'", traceId, requestModel.CedulaAsegurado));
                        throw new BPSegurosException((int)HttpStatusCode.Conflict, "La persona que desea guardar ya existe.");
                    }

                    PersonaEntity personaEntity = new()
                    {
                        CedulaAsegurado = requestModel.CedulaAsegurado,
                        IdTipoPersona = requestModel.IdTipoPersona,
                        FechaNacimiento = requestModel.FechaNacimiento,
                        Nombre = requestModel.Nombre,
                        PrimerApellido = requestModel.PrimerApellido,
                        SegundoApellido = requestModel.SegundoApellido,
                        EstaEliminado = false,
                    };

                    await dbContext.Persona.AddAsync(personaEntity);

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
                BPSegurosException argosEx = new BPSegurosException((int)HttpStatusCode.InternalServerError, "Ha ocurrido un error al crear la persona.", ex);
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