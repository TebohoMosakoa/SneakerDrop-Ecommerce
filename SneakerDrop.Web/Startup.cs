using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SneakerDrop.Web.Startup))]
namespace SneakerDrop.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
