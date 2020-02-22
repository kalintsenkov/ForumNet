using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(ForumNet.Web.Areas.Identity.IdentityHostingStartup))]
namespace ForumNet.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => 
            {
            });
        }
    }
}