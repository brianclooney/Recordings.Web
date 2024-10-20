using Microsoft.Extensions.Options;
using Prometheus;
using Recordings.API.Configuration;

namespace Recordings.API.Middleware
{
    public class StaticFileMetricsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly FilePathOptions _filePathOptions;
        private static readonly Counter StaticFileRequestsCounter = Metrics.CreateCounter("http_static_requests_received_total", "Number of static file requests", new CounterConfiguration
        {
            LabelNames = new[] { "code", "path" }
        });

        public StaticFileMetricsMiddleware(RequestDelegate next, IOptions<FilePathOptions> filePathOptionsAccessor)
        {
            _next = next;
            _filePathOptions = filePathOptionsAccessor.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            // Console.WriteLine("");
            // Console.WriteLine("StaticFileMetricsMiddleware.InvokeAsync: " + context.Response.StatusCode);
            // Console.WriteLine("StaticFileMetricsMiddleware.InvokeAsync: " + context.Request.Path.ToString());
            // Console.WriteLine("StaticFileMetricsMiddleware.InvokeAsync: " + _filePathOptions.StaticFileRequestPath);

            if (context.Response.StatusCode < 400 && context.Request.Path.ToString().StartsWith(_filePathOptions.StaticFileRequestPath))
            {
                StaticFileRequestsCounter.WithLabels(context.Response.StatusCode.ToString(), context.Request.Path).Inc();
            }
        }
    }
}
