using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Cors;
using Microsoft.Owin.Cors;
using System.Web.Http;
using WebApiAuthentication.Business.Common;

[assembly: OwinStartup(typeof(WebApiAuthentication.Api.App_Start.Startup))]

namespace WebApiAuthentication.Api.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // get current settings
            var settings = AppConfiguration.Current;
            // setup cors to accept all for now
            app.UseCors(CorsOptions.AllowAll);
            // create new http configuration and register web api configurations
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            // configure authenticate
            ConfigureAuthentication(app);

            //app.UseStageMarker(PipelineStage.Authenticate); // Needed for IIS pipeline
            // get owin to use the web api configuration
            app.UseWebApi(config);
        }
    }
}
