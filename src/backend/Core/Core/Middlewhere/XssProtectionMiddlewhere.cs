using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Middlewhere
{
    public class XssProtectionMiddlewhere
    {
        private readonly RequestDelegate _next;
		public XssProtectionMiddlewhere(RequestDelegate next)
		=> _next = next;

		public async Task InvokeAsync(HttpContext context)
		{
			context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
			context.Response.Headers.Add("X-Xss-Protection", "1");
			context.Response.Headers.Add("X-Frame-Options", "DENY");

			await _next.Invoke(context);
		}
	}
}
