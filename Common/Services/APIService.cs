using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Common.Services
{
    public class APIService
    {
        private readonly ILogger<APIService> _logger;

        public APIService(ILogger<APIService> logger)
        {
            _logger = logger;
        }

        private HttpClient InitHttpClient(string traceId, string apiEndpoint, string token)
        {
            try
            {
                _logger.LogInformation("{0} - Iniciando ejecución del método {1}", traceId,
                    MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name);

                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                httpClient.BaseAddress = new Uri(apiEndpoint);

                return httpClient;
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} - Excepción: {1}", traceId, ex.ToString());
                string msg = "Ocurrió un error al intentar conectarse con el servidor de destino.";
                throw new BPSegurosException(500, msg, ex);
            }
            finally
            {
                _logger.LogInformation("{0} - Finalizando ejecución del método {1}", traceId,
                    MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name);
            }
        }

        public async Task<HttpResponseMessage> GetHttpResponseMessage(string traceId, string baseUrlAddress, string methodPath, string token)
        {
            try
            {
                _logger.LogInformation("{0} - Iniciando ejecución del método {1}", traceId,
                    MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name);

                using (var httpClient = InitHttpClient(traceId, baseUrlAddress, token))
                {
                    return await httpClient.GetAsync(methodPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} - Excepción: {1}", traceId, ex.ToString());
                string msg = "Ocurrió un error al intentar conectarse con el servidor de destino.";
                throw new BPSegurosException(500, msg, ex);
            }
            finally
            {
                _logger.LogInformation("{0} - Finalizando ejecución del método {1}", traceId,
                    MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name);
            }
        }
    }
}
