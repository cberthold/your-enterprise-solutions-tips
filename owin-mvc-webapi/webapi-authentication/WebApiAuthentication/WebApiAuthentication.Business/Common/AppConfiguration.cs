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

        public bool AllowInsecureHttp { 
            get {
                return GetAndStoreValue(a => a.AllowInsecureHttp, (prop) =>
                {
                    return GetOrDefaultBoolean(prop);
                });
            } 
        }

        public string JwtSigningKey
        {
            get
            {
                return GetAndStoreValue(a => JwtSigningKey, (prop) =>
                {
                    return GetOrDefaultstring(prop, "random signing key");
                });
            }
        }
        
        private AppConfiguration() { }

        public static AppConfiguration Current
        {
            get
            {
                var config = new AppConfiguration();
                return config;
            }
        }

        
    }
}
