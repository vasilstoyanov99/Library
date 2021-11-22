using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Library.Areas.Identity.IdentityHostingStartup))]
namespace Library.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}