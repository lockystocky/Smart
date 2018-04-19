using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HelpDeskTeamProject.Startup))]
namespace HelpDeskTeamProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
