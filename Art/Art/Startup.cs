using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Art.Startup))]
namespace Art
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
