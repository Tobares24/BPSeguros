using Common.Entities;
using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seguridad.Entities;
using Seguridad.Models.CrearUsuario;
using System.Net;
using System.Reflection;

namespace Seguridad.Services
{
    public class CrearUsuarioService
    {
        private readonly ILogger<CrearUsuarioService> _logger;
        private readonly DbContextFactoryService _dbContextFactoryService;
        private readonly RequestModelValidationService _validationService;
        private readonly PasswordHasherService _passwordHasherService;
        private readonly JsonService _jsonService;

        public CrearUsuarioService(ILogger<CrearUsuarioService> logger, DbContextFactoryService dbContextFactoryService, JsonService jsonService,
            RequestModelValidationService validationService, PasswordHasherService passwordHasherService)
        {
            _logger = logger;
            _dbContextFactoryService = dbContextFactoryService;
            _jsonService = jsonService;
            _validationService = validationService;
            _passwordHasherService=passwordHasherService;
        }

        public async Task<IActionResult> Crear(HttpContext httpContext)
        {
            try
            {
                _logger.LogInformation(string.Format("{0} - Iniciando ejecución del método {1}", httpContext.TraceIdentifier, MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name));

                string traceId = httpContext.TraceIdentifier;

                CrearUsuarioRequestModel requestModel = await _jsonService.RequestToObjectAsync<CrearUsuarioRequestModel>(httpContext);

                _validationService.Validate(requestModel, traceId);

                using (var dbContext = _dbContextFactoryService.CreateDbContext<SeguridadDbContext>())
                {
                    var existeUsuario = await dbContext.Usuario.AnyAsync(x => x.Email == requestModel.Email);
                    if (existeUsuario)
                    {
                        _logger.LogError(string.Format("{0} - Ya existe un registro con el email '{1}'", traceId, requestModel.Email));
                        throw new BPSegurosException((int)HttpStatusCode.Conflict, "El email que desea registrar ya existe.");
                    }

                    var hashPassword = _passwordHasherService.HashPassword(requestModel.Password!);

                    UsuarioEntity usuarioEntity = new()
                    {
                        Email = requestModel.Email,
                        PasswordHash = hashPassword,
                    };

                    await dbContext.Usuario.AddAsync(usuarioEntity);

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
                BPSegurosException argosEx = new BPSegurosException((int)HttpStatusCode.InternalServerError, "Ha ocurrido un error al crear el usuario.", ex);
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
