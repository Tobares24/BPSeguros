using Common.Entities;
using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Poliza.Entities;
using Poliza.Models.ObtenerPoliza;
using System.Net;
using System.Reflection;

namespace Poliza.Services.ObtenerPoliza
{
    public class ObtenerPolizaService
    {
        private readonly ILogger<ObtenerPolizaService> _logger;
        private readonly DbContextFactoryService _dbContextFactoryService;
        private readonly InternalService _internalService;
        private readonly JsonService _jsonService;

        public ObtenerPolizaService(ILogger<ObtenerPolizaService> logger, DbContextFactoryService dbContextFactoryService, InternalService internalService, JsonService jsonService)
        {
            _logger = logger;
            _dbContextFactoryService = dbContextFactoryService;
            _internalService = internalService;
            _jsonService = jsonService;
        }

        public async Task<IActionResult> Obtener(HttpContext httpContext)
        {
            try
            {
                _logger.LogInformation(string.Format("{0} - Iniciando ejecución del método {1}", httpContext.TraceIdentifier, MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name));

                string traceId = httpContext.TraceIdentifier;

                ObtenerPolizaResponseModel responseModel = new()
                {
                    Registros = new(),
                };

                List<PolizaEntity> registros = new();

                string? numeroPoliza = httpContext.Request.Query["numeroPoliza"];
                string? cedulaAsegurado = httpContext.Request.Query["cedulaAsegurado"];
                string? fechaVencimiento = httpContext.Request.Query["fechaVencimiento"];
                string? idTipoPoliza = httpContext.Request.Query["idTipoPoliza"];
                string? paginaActualParams = httpContext.Request.Query["paginaActual"];
                string? registroPorPaginaParams = httpContext.Request.Query["registroPorPagina"];

                int paginaActual = int.Parse(paginaActualParams ?? "1");
                int registroPorPagina = int.Parse(registroPorPaginaParams ?? "10");

                using (var dbContext = _dbContextFactoryService.CreateDbContext<PolizaDbContext>())
                {
                    var query = dbContext.Poliza.Include(x => x.TipoPoliza).Include(x => x.PolizaEstado).Where(x => !x.EstaEliminado && x.TipoPoliza != null && x.PolizaEstado != null);

                    query = !string.IsNullOrEmpty(numeroPoliza) ? query.Where(x => x.NumeroPoliza!.ToLower().StartsWith(numeroPoliza)) : query;
                    query = !string.IsNullOrEmpty(cedulaAsegurado) ? query.Where(x => x.CedulaAsegurado!.ToLower().StartsWith(cedulaAsegurado)) : query;
                    if (DateTime.TryParse(fechaVencimiento, out var fecha))
                    {
                        query = query.Where(x => x.FechaVencimiento.Date == fecha.Date);
                    }

                    query = !string.IsNullOrEmpty(idTipoPoliza) ? query.Where(x => x.IdTipoPoliza == Guid.Parse(idTipoPoliza)) : query;

                    responseModel.CantidadRegistrosPaginas = await query.CountAsync();
                    responseModel.CantidadPaginas = (int)Math.Ceiling((decimal)responseModel.CantidadRegistrosPaginas / (decimal)registroPorPagina);

                    registros = await query
                         .Skip((paginaActual - 1) * registroPorPagina)
                         .Take(registroPorPagina)
                         .ToListAsync();
                }

                if (registros.Any())
                {
                    List<Task<object?>> tareas = new();
                    foreach (var item in registros)
                    {
                        tareas.Add(_internalService.GetEntityById<object>(httpContext.TraceIdentifier, "api/personas", item.CedulaAsegurado!, "persona"));
                    }

                    var listaPersonasObtenidas = await Task.WhenAll(tareas);

                    List<ObtenerPorIdResponseModel> obtenerPorIdResponseModels = new();
                    foreach (var personaResponseJson in listaPersonasObtenidas)
                    {
                        ObtenerPorIdResponseModel personaObtenida = _jsonService.ConvertToObject<ObtenerPorIdResponseModel>(personaResponseJson!.ToString()!);
                        obtenerPorIdResponseModels.Add(personaObtenida);
                    }

                    responseModel.Registros = registros.Select(poliza =>
                    {
                        var personaObtenida = obtenerPorIdResponseModels.FirstOrDefault(x => x.CedulaAsegurado == poliza.CedulaAsegurado);
                        return new ObtenerPolizaModel
                        {
                            CedulaAsegurado = poliza.CedulaAsegurado,
                            FechaVencimiento = poliza.FechaVencimiento,
                            NombreCompleto = $"{personaObtenida?.Nombre ?? ""} {personaObtenida?.PrimerApellido ?? ""} {personaObtenida?.SegundoApellido ?? ""}",
                            Id = poliza.Id,
                            NumeroPoliza = poliza.NumeroPoliza,
                            TipoPoliza = poliza.TipoPoliza?.Descripcion,
                            PolizaEstado = poliza.PolizaEstado?.Descripcion,
                        };
                    }).ToList();
                }

                responseModel.PaginaActual = paginaActual;
                responseModel.CantidadRegistros = responseModel.Registros.Count();

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
                BPSegurosException argosEx = new BPSegurosException((int)HttpStatusCode.InternalServerError, "Ha ocurrido un error al obtener las pólizas.", ex);
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
