using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace CRUD_Operations.ActionFilter
{
    public class LogActivityFilter : IActionFilter,IAsyncActionFilter
    {
        private readonly ILogger<LogActivityFilter> _logger;

        public LogActivityFilter(ILogger<LogActivityFilter> logger)
        {
            _logger = logger;
        }
        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"Executing action {context.ActionDescriptor.DisplayName} on controller {context.Controller} with parameter {JsonSerializer.Serialize( context.ActionArguments)}");
        }
        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"Action {context.ActionDescriptor.DisplayName} finished executed on controller {context.Controller}");
        }

        //when impelement the two interfaces it by default use the [IAsyncActionFilter] and don't use [IActionFilter]
        async Task IAsyncActionFilter.OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation($"(Async) Executing action {context.ActionDescriptor.DisplayName} on controller {context.Controller} with parameter {JsonSerializer.Serialize(context.ActionArguments)}");

            await next();

            _logger.LogInformation($"(Async) Action {context.ActionDescriptor.DisplayName} finished executed on controller {context.Controller}");

        }

    }
}
