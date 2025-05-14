using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Common.Services
{
    public class JsonService
    {
        private readonly ILogger<JsonService> _logger;

        public JsonService(ILogger<JsonService> logger)
        {
            _logger = logger;
        }

        public async Task<T> RequestToObjectAsync<T>(HttpContext httpContext)
        {
            try
            {
                _logger.LogInformation($"{httpContext.TraceIdentifier} - Iniciando ejecución del método {MethodBase.GetCurrentMethod()!.ReflectedType!.FullName}.{MethodBase.GetCurrentMethod()!.Name}");

                httpContext.Request.EnableBuffering();
                string json;
                using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8))
                {
                    json = await reader.ReadToEndAsync().ConfigureAwait(false);
                }

                return JsonSerializer.Deserialize<T>(json)!;
            }
            catch (JsonException ex)
            {
                _logger.LogError($"{httpContext.TraceIdentifier} - JsonException: {ex.ToString()}");
                throw new BPSegurosException(500, "Error de deserialización de JSON", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{httpContext.TraceIdentifier} - ArgosException: {ex.ToString()}");
                throw new BPSegurosException(500, "An error occurred while trying to convert the request to an object", ex);
            }
            finally
            {
                _logger.LogInformation($"{httpContext.TraceIdentifier} - Finalizando ejecución del método {MethodBase.GetCurrentMethod()!.ReflectedType!.FullName}.{MethodBase.GetCurrentMethod()!.Name}");
            }
        }
    }
}