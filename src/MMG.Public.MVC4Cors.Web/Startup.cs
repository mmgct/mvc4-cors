using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MMG.Public.MVC4Cors.Web.Startup))]
namespace MMG.Public.MVC4Cors.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
