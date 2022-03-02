﻿using System.Text.Json;

namespace Core.Middlewhere
{
    public class ExceptionMiddlewhere
    {
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _webHostEnv;
        private readonly RequestDelegate _next;
        
        public ExceptionMiddlewhere(ILoggerFactory loggerFactory,
            IWebHostEnvironment webHostEnv,
            RequestDelegate next)
        {
            _logger = loggerFactory.CreateLogger(nameof(ExceptionMiddlewhere));
            _webHostEnv = webHostEnv;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError("Something was error", ex);

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                string json = "";
                if (_webHostEnv.IsProduction())
                {
                    json = JsonSerializer.Serialize(new
                    {
                        exception = "Something was error, try later"
                    });
                    await context.Response.WriteAsync(json);
                }
                else
                {
                    json = JsonSerializer.Serialize(ex);
                    await context.Response.WriteAsync(json);
                }
            }
        }
    }
}