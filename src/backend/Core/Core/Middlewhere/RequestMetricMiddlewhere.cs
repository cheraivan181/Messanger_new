using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Prometheus;

namespace Core.Middlewhere;

public class RequestMetricMiddlewhere
{
    private readonly RequestDelegate _next;
    private readonly Counter _requestCounter;
    
    public RequestMetricMiddlewhere(RequestDelegate next)
    {
        _next = next;
        _requestCounter = Metrics
            .CreateCounter("total_requests", "The total number of requests serviced by this API.");
    }

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path;
        var method = context.Request.Method;
        
        try
        {
            await _next.Invoke(context);
        }
        catch
        {
            throw;
        }

        if (path != "/metrics")
        {
            _requestCounter.Inc();
        }
    }
}
