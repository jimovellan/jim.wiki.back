using jim.wiki.core.Errors;
using jim.wiki.core.Exceptions;
using jim.wiki.core.Results;
using Newtonsoft.Json;

namespace jim.wiki.back.api.Middlewares
{
    public class HandlingExceptionMidleware
    {
        public readonly RequestDelegate _next;
        private readonly ILogger<HandlingExceptionMidleware> _logger;

        public HandlingExceptionMidleware(RequestDelegate next, ILogger<HandlingExceptionMidleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(CustomDomainValidationException validationException)
            {
                var resultError = Result.Fail(new Error("VALIDATION_ERROR", "Hubo errores de validación"));
               
                resultError.AddValidations(validationException.Errors.ToArray());

                await httpContext.Response.WriteAsJsonAsync(resultError);
                
            }
            catch(CustomDomainExceptions custonDomain)
            {
                var resultError = Result.Fail(custonDomain.Errors.ToArray());

                await httpContext.Response.WriteAsJsonAsync(resultError);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                
                var resultError = Result.Fail(new Error("GENERIC_ERROR", "La operación finalizo con errores"));
                await httpContext.Response.WriteAsJsonAsync(resultError);
            }
        }
    }
}
