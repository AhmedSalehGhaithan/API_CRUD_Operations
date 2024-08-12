using System.Diagnostics;

namespace CRUD_Operations.Middleware
{
    public class ProfilingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ProfilingMiddleware> logger;

        public ProfilingMiddleware(RequestDelegate next,ILogger<ProfilingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await next(context);
            stopwatch.Stop();
            logger.LogInformation($"Request '{context.Request.Path}' took '{stopwatch.ElapsedMilliseconds}ms' to execute.");
        }
    }
}
