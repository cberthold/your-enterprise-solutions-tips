using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiAuthentication.Common.Configuration;

namespace WebApiAuthentication.Business.Common
{
    public sealed class AppConfiguration : BaseConfiguration<AppConfiguration>
    {

        private AppConfiguration() { }

        public static AppConfiguration Current
        {
            get
            {
                var config = new AppConfiguration();
                return config;
            }
        }

        public bool AllowInsecureHttp { get { return GetAndStoreValue(a => a.AllowInsecureHttp, false); } }

        public string TokenEndpointPath { get { return GetAndStoreValue(a => TokenEndpointPath, "/auth/token"); } }

        public double AccessTokenExpireTimeSpan { get { return GetAndStoreValue(a => a.AccessTokenExpireTimeSpan, 1800); } }
    }
}
