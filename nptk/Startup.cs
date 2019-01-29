using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(nptk.Startup))]
namespace nptk
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
