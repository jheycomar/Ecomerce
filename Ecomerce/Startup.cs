using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ecomerce.Startup))]
namespace Ecomerce
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
