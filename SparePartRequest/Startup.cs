using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SparePartRequest.Startup))]
namespace SparePartRequest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
