using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Vessel_Info.Web.Areas.Identity.IdentityHostingStartup))]
namespace Vessel_Info.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder) 
            => builder.ConfigureServices((context, services) => {});
    }
}