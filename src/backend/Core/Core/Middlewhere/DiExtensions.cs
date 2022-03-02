namespace Core.Middlewhere
{
    public static class DiExtensions
    {
        public static void UseMiddleWhares(this IApplicationBuilder app)
        {
            app.UseMiddleware<XssProtectionMiddlewhere>();
            app.UseMiddleware<ExceptionMiddlewhere>();
        }
    }
}
