using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CodeEditorApp.Startup))]
namespace CodeEditorApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
