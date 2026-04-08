using Microsoft.Extensions.DependencyInjection;

namespace ContrackAPI
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services)
        {
            // Controllers
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidateUserStatusFilter>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            services.AddEndpointsApiExplorer();
            services.AddHttpContextAccessor();

            // CORS configuration
            services.AddCors(options =>
            {
                options.AddPolicy("MyDomainPolicy", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}