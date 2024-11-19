namespace API.Extensions
{
    public static class MiddlewareConfiguration
    {
        public static void ConfigureMiddleware(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleware();
            }

            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
