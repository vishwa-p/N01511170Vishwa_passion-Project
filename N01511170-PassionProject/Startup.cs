using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(N01511170_PassionProject.Startup))]
namespace N01511170_PassionProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
