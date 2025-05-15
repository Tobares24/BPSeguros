using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Common.Services
{
    public class RequestModelValidationService
    {
        private readonly ILogger<RequestModelValidationService> _logger;

        public RequestModelValidationService(ILogger<RequestModelValidationService> logger)
        {
            _logger = logger;
        }

        public void Validate<T>(T obj, string traceId)
        {
            try
            {
                this._logger.LogInformation(string.Format("{0} - Iniciando ejecución del método {1}", traceId,
                    MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name));

                List<ValidationResult> errors = new List<ValidationResult>();
                if (!Validator.TryValidateObject(obj!, new ValidationContext(obj!), errors, true))
                {
                    throw new BPSegurosException(400, errors.FirstOrDefault()!.ErrorMessage!, fieldName: errors.FirstOrDefault()!.MemberNames.FirstOrDefault());
                }
            }
            catch (BPSegurosException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - ArgosException: {1}", traceId, ex.ToString()));
                string targetText = "An error occurred while trying to validate the request";
                throw new BPSegurosException(500, targetText, ex);
            }
            finally
            {
                _logger.LogInformation(string.Format("{0} - Finalizando ejecución del método {1}", traceId,
                    MethodBase.GetCurrentMethod()!.ReflectedType!.FullName + "." + MethodBase.GetCurrentMethod()!.Name));
            }
        }
    }
}