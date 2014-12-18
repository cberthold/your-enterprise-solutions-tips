using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiAuthentication.Business.Common;
using WebApiAuthentication.Business.OAuth;

namespace WebApiAuthentication.Api.App_Start
{
    public partial class Startup
    {
        private void ConfigureAuthentication(IAppBuilder app)
        {
            var config = AppConfiguration.Current;

            // setup OAuth Authorization server options
            var serverOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = config.AllowInsecureHttp,
                TokenEndpointPath = new PathString(config.TokenEndpointPath),
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(config.AccessTokenExpireTimeSpan),
                Provider = new CustomOAuthAuthorizationServerProvider()
            };

            // add authentication and token endpoints to owin for use with web api
            app.UseOAuthAuthorizationServer(serverOptions);
        }
    }
}