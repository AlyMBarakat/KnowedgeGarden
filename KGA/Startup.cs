using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KGA.Startup))]
namespace KGA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
