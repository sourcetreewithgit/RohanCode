using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GST_Impl.Startup))]
namespace GST_Impl
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
