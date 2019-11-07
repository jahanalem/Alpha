using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Alpha.Infrastructure
{
    public static class DynamicOperation
    {
        public static List<string> GetNamesOfProperties(Type typeOf, bool exceptOfICollection = true)
        {
            var propertyNames = new List<string>();
            var properties = typeOf.GetProperties();
            //var props = new List<PropertyInfo>();
            foreach (PropertyInfo prop in properties)
            {
                Type type = prop.PropertyType;

                bool thePropertyIsNotTypeOfICollection = true;
                if (exceptOfICollection)
                {
                    thePropertyIsNotTypeOfICollection = !(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>));
                }
                if (thePropertyIsNotTypeOfICollection)
                {
                    string attrName;
                    var attribute = (DisplayNameAttribute)prop.GetCustomAttribute(typeof(DisplayNameAttribute), true);
                    if (attribute != null)
                    {
                        attrName = attribute.DisplayName;
                    }
                    else
                    {
                        attrName = prop.Name;
                        //props.Add(prop);
                    }
                    propertyNames.Add(attrName);
                }
            }
            return propertyNames;
        }

        public static Dictionary<string, object> GetProperties(this object obj)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            List<PropertyInfo> properties = obj.GetType().GetProperties().ToList();
            foreach (var prop in properties)
            {
                object val = prop.GetValue(obj, null);
                string name = prop.Name;
                result.Add(name, val);
            }
            return result;
        }
    }
}
