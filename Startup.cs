using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TaskDone_V2.Startup))]
namespace TaskDone_V2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
