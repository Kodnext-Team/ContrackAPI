using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ContrackAPI
{
    public static class ApplicationConfiguration    
    {
        // Register all services
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
        {
            services.AddServiceConfiguration();
            services.AddSwaggerConfiguration();
            services.ConfigureServices(); 
            return services;
        }

        public static WebApplication UseApplicationConfiguration(this WebApplication app)
        {
            app.UseMiddlewareConfiguration(); 
            return app;
        }
    }
}