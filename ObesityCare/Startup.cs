using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ObesityCare.Startup))]
namespace ObesityCare
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
