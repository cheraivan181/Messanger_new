using Prometheus;

namespace Core.Middlewhere;

public class RequestMetricMiddlewhere
{
    private readonly RequestDelegate _next;

    public RequestMetricMiddlewhere(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path;
        var method = context.Request.Method;

        var counter = Metrics.CreateCounter("request_totals", "Http request totals", new CounterConfiguration()
        {
            LabelNames = new[] {"path", "method"}
        });

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
            counter.Labels(path, method);
        }
    }
}
