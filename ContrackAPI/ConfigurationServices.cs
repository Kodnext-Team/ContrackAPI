namespace ContrackAPI
{
    public static class ConfigurationServices
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            // Business Services
            services.AddScoped<IVoyageService, VoyageService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IContainerService, ContainerService>();
            services.AddScoped<ITrackingService, TrackingService>();

            // Repository
            services.AddScoped<IVoyageRepository, VoyageRepository>();
            services.AddScoped<ILoginRepository, LoginRepository>();         
            services.AddScoped<IHubRepository, HubRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IContainerRepository, ContainerRepository>();
            services.AddScoped<ITrackingRepository, TrackingRepository>();
        }
    }
}