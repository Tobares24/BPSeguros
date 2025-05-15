using Common.Models;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.Json;

namespace Common.Services
{
    public class InternalService
    {
        private readonly ILogger<InternalService> _logger;
        private readonly APIService _apiService;

        public InternalService(ILogger<InternalService> logger, APIService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public async Task<T?> GetEntityById<T>(string traceId, string controller, string id, string microService)
        {
            try
            {
                _logger.LogInformation(string.Format("{0} - Iniciando ejecución del método {1}", traceId, MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name));

                string? baseUrl = microService switch
                {
                    "persona" => Environment.GetEnvironmentVariable("PERSONA_ENDPOINT") ?? "https://localhost:7257",
                    "poliza" => Environment.GetEnvironmentVariable("POLIZA_ENDPOINT") ?? "https://localhost:7029",
                    _ => null,
                };

                if (baseUrl == null)
                {
                    throw new BPSegurosException(400, "No se ha configurado la URL.");
                }

                HttpResponseMessage responseMessage = await _apiService.GetHttpResponseMessage(traceId, baseUrl, $"/{controller}/{id}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    string json = await responseMessage.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(json);
                }

                try
                {
                    string json = await responseMessage.Content.ReadAsStringAsync();
                    ErrorResponseModel error = JsonSerializer.Deserialize<ErrorResponseModel>(json)!;
                    throw new BPSegurosException(error.StatusCode!.Value, error.Message!, new Exception($"{traceId} - BPSegurosException: {error.TraceId} - {error.Message}"));
                }
                catch (BPSegurosException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format("{0} - Exception: {1}", traceId, ex.ToString()));
                    throw;
                }
            }
            catch (BPSegurosException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - BPSegurosException: {1}", traceId, ex.ToString()));
                throw new BPSegurosException(500, "Ha ocurrido un error al obtener la entidad");
            }
            finally
            {
                _logger.LogInformation(string.Format("{0} - Finalizando ejecución del método {1}", traceId,
                    MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name));
            }
        }
    }
}
