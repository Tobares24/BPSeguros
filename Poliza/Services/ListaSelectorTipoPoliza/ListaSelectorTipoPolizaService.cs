﻿using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Poliza.Entities;
using Poliza.Models.ListaSelectorTipoPoliza;
using System.Net;
using System.Reflection;

namespace Poliza.Services.ListaSelectorTipoPoliza
{
    public class ListaSelectorTipoPolizaService
    {
        private readonly ILogger<ListaSelectorTipoPolizaService> _logger;
        private readonly DbContextFactoryService _dbContextFactoryService;

        public ListaSelectorTipoPolizaService(ILogger<ListaSelectorTipoPolizaService> logger, DbContextFactoryService dbContextFactoryService)
        {
            _logger = logger;
            _dbContextFactoryService = dbContextFactoryService;
        }

        public async Task<IActionResult> ListaSelectorTipoPoliza(HttpContext httpContext)
        {
            try
            {
                _logger.LogInformation(string.Format("{0} - Iniciando ejecución del método {1}", httpContext.TraceIdentifier, MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name));

                string traceId = httpContext.TraceIdentifier;

                string? filtro = httpContext.Request.Query["filtro"];

                List<ListaSelectorTipoPolizaResponseModel> registros = new();

                using (var dbContext = _dbContextFactoryService.CreateDbContext<PolizaDbContext>())
                {
                    var query = dbContext.TipoPoliza.Where(x => !x.EstaEliminado);

                    query = !string.IsNullOrEmpty(filtro) ? query.Where(x => x.Descripcion != null && x.Descripcion.StartsWith(filtro)) : query;

                    registros = await query
                         .Take(10)
                         .Select(x => new ListaSelectorTipoPolizaResponseModel
                         {
                             Id = x.Id,
                             Descripcion = x.Descripcion,
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
                BPSegurosException argosEx = new BPSegurosException((int)HttpStatusCode.InternalServerError, "Ha ocurrido un error al obtener los tipos de pólizas del selector.", ex);
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