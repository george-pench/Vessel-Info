namespace Vessel_Info.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Data.Models;

    using static Constants.WebConstants;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder DatabaseInit(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            var db = services.GetRequiredService<VesselInfoDbContext>();
            db.Database.EnsureCreated();

            SeedAdmin(services);

            return app;
        } 

        private static void SeedAdmin(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole
                    {
                        Name = AdministratorRoleName
                    };

                    await roleManager.CreateAsync(role);

                    var user = new User
                    {
                        Email = AdminEmail,
                        UserName = AdminEmail,
                        Name = AdminUserName
                    };

                    await userManager.CreateAsync(user, AdminPassword);
                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
