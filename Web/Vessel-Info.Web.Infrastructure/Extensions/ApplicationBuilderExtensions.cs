namespace Vessel_Info.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Vessel_Info.Data;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder DatabaseInit(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            var db = services.GetRequiredService<VesselInfoDbContext>();
            db.Database.Migrate();

            return app;
        } 
    }
}
