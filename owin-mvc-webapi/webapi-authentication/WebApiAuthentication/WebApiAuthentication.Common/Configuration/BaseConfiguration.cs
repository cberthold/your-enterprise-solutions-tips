using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApiAuthentication.Common.Reflection;

namespace WebApiAuthentication.Common.Configuration
{
    public abstract class BaseConfiguration<TConfig>
        where TConfig : BaseConfiguration<TConfig>
    {
        private ConcurrentDictionary<string, object> cacheObject = new ConcurrentDictionary<string, object>();
        private Type thisType = typeof(TConfig);

        protected T GetAndStoreValue<T>(Expression<Func<TConfig, T>> configProperty, Func<T> action)
        {
            // get the configuration property
            var property = ReflectionHelper.FindProperty(configProperty);

            if (property == null)
            {
                throw new ArgumentException("configProperty expression must evaluate to a property of the configuration class");
            }
            // get the property name from the memberinfo
            var propertyName = property.Name;

            // try or get property via action
            var returnValue = (T)cacheObject.GetOrAdd(propertyName, (p) =>
            {  
                return action.Invoke();
            });
            
            return returnValue;

        }



        protected int GetOrDefaultInteger(string key, int defaultValue = 0)
        {
            int? intValue = GetInteger(key);

            if (!intValue.HasValue)
            {
                intValue = defaultValue;
            }

            return intValue.Value;
        }

        protected int? GetInteger(string key)
        {
            string stringValue = GetApplicationKey(key);

            int parsedValue;

            if (int.TryParse(stringValue, out parsedValue))
            {
                return parsedValue;
            }

            return null;
        }

        protected bool GetOrDefaultBoolean(string key, bool defaultValue = false)
        {
            bool? boolValue = GetBoolean(key);

            if (!boolValue.HasValue)
            {
                boolValue = defaultValue;
            }

            return boolValue.Value;
        }

        protected bool? GetBoolean(string key)
        {
            string stringValue = GetApplicationKey(key);

            bool parsedValue;

            if (bool.TryParse(stringValue, out parsedValue))
            {
                return parsedValue;
            }

            return null;
        }

        protected Guid GetOrDefaultGuid(string key, Guid defaultValue)
        {
            Guid? guidValue = GetGuid(key);

            if (!guidValue.HasValue)
            {
                guidValue = defaultValue;
            }

            return guidValue.Value;
        }

        protected Guid? GetGuid(string key)
        {
            string stringValue = GetApplicationKey(key);

            Guid parsedValue;

            if (Guid.TryParse(stringValue, out parsedValue))
            {
                return parsedValue;
            }

            return null;
        }

        protected string GetOrDefaultstring(string key, string defaultValue = null)
        {
            string stringValue = GetString(key);

            if (stringValue == null)
            {
                stringValue = defaultValue;
            }

            return stringValue;
        }

        protected string GetString(string key)
        {
            string stringValue = GetApplicationKey(key);

            return stringValue;
        }

        protected decimal GetOrDefaultDecimal(string key, decimal defaultValue)
        {
            decimal? decimalValue = GetDecimal(key);

            if (!decimalValue.HasValue)
            {
                decimalValue = defaultValue;
            }

            return decimalValue.Value;
        }

        protected decimal? GetDecimal(string key)
        {
            string stringValue = GetApplicationKey(key);

            decimal parsedValue;

            if (decimal.TryParse(stringValue, out parsedValue))
            {
                return parsedValue;
            }

            return null;
        }

        protected string GetApplicationKey(string key)
        {
            string returnValue = ConfigurationManager.AppSettings[key];

            return returnValue;
        }
    }
}
