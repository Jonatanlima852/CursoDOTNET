using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiCatalogo.Filters
{
    public class ApiLoggingFilter : IActionFilter //implementando a interface padrão que contém os métodos
    {
        private readonly ILogger<ApiLoggingFilter> _logger; // a interface logger permite registrar infos, msgs e eventos na saída do console

        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger) //construtor para lidar com injeção de dependência
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context) //método a ser executado antes da action
        {
            // objeto context permite acesso a controller, httpcontext, modelstate, result...
            _logger.LogInformation("### Executando -> OnActionExecuting");
            _logger.LogInformation("##########################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation($"ModelState: {context.ModelState.IsValid}");
            _logger.LogInformation("##########################################");
        }

        public void OnActionExecuted(ActionExecutedContext context) //método a ser executado após a action
        {
            _logger.LogInformation("### Executou -> OnActionExecuted");
            _logger.LogInformation("##########################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation($"Http Status Code: {context.HttpContext.Response.StatusCode}");
            _logger.LogInformation("##########################################");
        }
    }
}