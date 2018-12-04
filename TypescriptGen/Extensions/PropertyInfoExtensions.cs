using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace System.Reflection
{
    public static class PropertyInfoExtensions
    {
        public static string TsType(this PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.TsType();
        }

        public static object TryGetConstant(this PropertyInfo propertyInfo)
        {
            var dictionary = propertyInfo.DeclaringType.GetConstants();
            var key = dictionary.Keys.FirstOrDefault(x => x.Name == propertyInfo.Name);
            return key == null ? null : dictionary[key];
        }

        public static IEnumerable<PropertyInfo> PropertyFilter(this IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos.Where(x => 
                !x.PropertyType.IsGenericType || 
                x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() != typeof(IObservable<>));
        }
    }
}