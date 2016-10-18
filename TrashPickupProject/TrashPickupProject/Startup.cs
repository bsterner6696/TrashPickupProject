using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrashPickupProject.Startup))]
namespace TrashPickupProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
