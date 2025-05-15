using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seguridad.Entities;
using System.Net;
using System.Reflection;

namespace Seguridad.Services.Login
{
    public class LoginService
    {
        private readonly ILogger<LoginService> _logger;
        private readonly PasswordHasherService _passwordHasherService;
        private readonly DbContextFactoryService _dbContextFactoryService;
        private readonly JWTService _jWTService;

        public LoginService(ILogger<LoginService> logger, PasswordHasherService passwordHasherService, DbContextFactoryService dbContextFactoryService, JWTService jWTService)
        {
            _logger = logger;
            _passwordHasherService = passwordHasherService;
            _dbContextFactoryService = dbContextFactoryService;
            _jWTService = jWTService;
        }

        public async Task<IActionResult> Login(HttpContext httpContext)
        {
            try
            {
                _logger.LogInformation(string.Format("{0} - Iniciando ejecución del método {1}", httpContext.TraceIdentifier, MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name));

                string traceId = httpContext.TraceIdentifier;

                string? email = httpContext.Request.Query["email"];
                string? password = httpContext.Request.Query["password"];

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    throw new BPSegurosException((int)HttpStatusCode.BadRequest, "Debe proporcionar el email y contraseña.");
                }

                using (var dbContext = _dbContextFactoryService.CreateDbContext<SeguridadDbContext>())
                {
                    var usuario = await dbContext.Usuario.FirstOrDefaultAsync(x => x.Email == email);
                    if (usuario is null)
                    {
                        _logger.LogError(string.Format("{0} - El email no ha sido encontrado '{1}'", traceId, email));
                        throw new BPSegurosException((int)HttpStatusCode.Conflict, "El usuario no ha sido encontrado.");
                    }

                    bool isPasswordValid = _passwordHasherService.VerifyPassword(usuario.PasswordHash!, password!);
                    if (!isPasswordValid)
                    {
                        _logger.LogError(string.Format("{0} - La contraseña es incorrecta '{1}'", traceId, password));
                        throw new BPSegurosException((int)HttpStatusCode.Conflict, "La contraseña es incorrecta.");
                    }

                    string token = _jWTService.GenerarToken(usuario);

                    return new ObjectResult(new
                    {
                        Token = token,
                        UsuarioId = usuario.Id,
                        Email = usuario.Email,
                    });
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
                BPSegurosException argosEx = new BPSegurosException((int)HttpStatusCode.InternalServerError, "Ha ocurrido un error al iniciar sesión.", ex);
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
