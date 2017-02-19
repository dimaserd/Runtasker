using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Runtasker.Startup))]
namespace Runtasker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
