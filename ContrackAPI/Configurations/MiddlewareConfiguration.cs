using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing; // Needed for MapControllers()
using Microsoft.Extensions.DependencyInjection;

namespace ContrackAPI
{
    public static class MiddlewareConfiguration
    {
        //public static WebApplication UseMiddlewareConfiguration(this WebApplication app)
        //{
        //    app.UseCors("MyDomainPolicy");
        //    app.UseSwagger(c =>
        //    {
        //        c.RouteTemplate = "swagger/{documentName}/swagger.json";
        //    });
        //    app.UseSwaggerUI(c =>
        //    {
        //        c.RoutePrefix = string.Empty;
        //        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        //        c.InjectStylesheet("/swagger-ui/custom.css");
        //    });
        //    app.UseHttpsRedirection();
        //    app.UseRouting();
        //    app.UseMiddleware<JwtMiddleware>();
        //    app.UseAuthentication();
        //    app.UseAuthorization();
        //    app.MapControllers();
        //    app.UseSwagger();
        //    app.UseSwaggerUI();
        //    return app;
        //}
        public static WebApplication UseMiddlewareConfiguration(this WebApplication app)
        {
            app.UseCors("MyDomainPolicy");

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                c.InjectStylesheet("/swagger-ui/custom.css");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseMiddleware<JwtMiddleware>();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}