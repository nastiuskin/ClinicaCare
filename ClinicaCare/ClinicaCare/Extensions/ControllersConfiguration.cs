namespace API.Extensions
{
    public static class ControllersConfiguration
    {
        public static IServiceCollection InitializeControllers(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
            });

            return services;
        }
        
    }
}
