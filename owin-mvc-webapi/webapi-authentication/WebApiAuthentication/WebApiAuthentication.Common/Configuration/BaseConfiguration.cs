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

        protected T GetAndStoreValue<T>(Expression<Func<TConfig, T>> configProperty, T defaultValue = default(T))
        {
            // try or get property via action
            Func<string, T> action = (prop) =>
            {
                object value = defaultValue;

                var type = typeof(T);
                var valueType = type;
                var isNullable = false;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    valueType = type.GetGenericArguments()[0];
                    isNullable = true;
                }

                if (valueType == typeof(string))
                {
                    value = GetString(prop);
                }
                else if (valueType == typeof(int))
                {
                    value = GetInteger(prop);
                }
                else if (valueType == typeof(bool))
                {
                    value = GetBoolean(prop);
                }
                else if (valueType == typeof(decimal))
                {
                    value = GetDecimal(prop);
                }
                else if (valueType == typeof(double))
                {
                    value = GetDouble(prop);
                }
                else if (valueType == typeof(Guid))
                {
                    value = GetGuid(prop);
                }
                else if (valueType == typeof(float))
                {
                    value = GetFloat(prop);
                }
                else
                {
                    throw new InvalidOperationException();
                }

                // value returned null so check if we should set default value
                if (value == null)
                {
                    if (isNullable && defaultValue == null)
                        return default(T);

                    value = defaultValue;
                }

                return (T)value;
            };

            return GetAndStoreValue(configProperty, action);

        }

        protected T GetAndStoreValue<T>(Expression<Func<TConfig, T>> configProperty, Func<string, T> action)
        {
            // get the configuration property
            var property = ReflectionHelper.FindProperty(configProperty);

            if (property == null)
            {
                throw new ArgumentException("configProperty expression must evaluate to a property of the configuration class");
            }
            // get the property name from the memberinfo
            var propertyName = property.Name;

            // get the cached value or create new value via action passed in
            T returnValue = (T)cacheObject.GetOrAdd(propertyName, (prop) =>
            {
                return action.Invoke(prop);
            });

            return returnValue;

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

        protected string GetString(string key)
        {
            string stringValue = GetApplicationKey(key);

            return stringValue;
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

        protected double? GetDouble(string key)
        {
            string stringValue = GetApplicationKey(key);

            double parsedValue;

            if (double.TryParse(stringValue, out parsedValue))
            {
                return parsedValue;
            }

            return null;
        }

        protected float? GetFloat(string key)
        {
            string stringValue = GetApplicationKey(key);

            float parsedValue;

            if (float.TryParse(stringValue, out parsedValue))
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
