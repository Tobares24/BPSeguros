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

        private HttpClient InitHttpClient(string traceId, string apiEndpoint, string userId, string businessIds, string accountIds)
        {
            try
            {
                _logger.LogInformation("{0} - Iniciando ejecución del método {1}", traceId,
                    MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name);

                var authtoken = GetToken(traceId);
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authtoken);
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

        private string GetToken(string traceId)
        {
            try
            {
                _logger.LogInformation("{0} - Iniciando ejecución del método {1}", traceId,
                    MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name);

                return "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTY4Mzg0MjUxNiwiZXhwIjoxNjgzODQ2MTE2fQ.3RMhs0XC5uoBBHVrvK8-gdX9DbXXLxwfQjkewcU2DQ0";
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} - Excepción: {1}", traceId, ex.ToString());
                string msg = "Ocurrió un error al intentar obtener el token de autenticación.";
                throw new BPSegurosException(500, msg, ex);
            }
            finally
            {
                _logger.LogInformation("{0} - Finalizando ejecución del método {1}", traceId,
                    MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name);
            }
        }

        public async Task<HttpResponseMessage> GetHttpResponseMessage(string traceId, string baseUrlAddress, string methodPath, string userId = "", string businessIds = "", string accountIds = "")
        {
            try
            {
                _logger.LogInformation("{0} - Iniciando ejecución del método {1}", traceId,
                    MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name);

                using (var httpClient = InitHttpClient(traceId, baseUrlAddress, userId, businessIds, accountIds))
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
