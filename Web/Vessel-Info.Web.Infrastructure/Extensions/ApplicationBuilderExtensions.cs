namespace Vessel_Info.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Vessel_Info.Data;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder DatabaseInit(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var service = serviceScope.ServiceProvider;

            var db = service.GetRequiredService<VesselInfoDbContext>();
            db.Database.EnsureCreated();

            return app;
        }
    }
}
