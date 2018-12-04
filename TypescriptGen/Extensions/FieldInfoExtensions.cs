using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace System.Reflection
{
    public static class FieldInfoExtensions
    {
        public static string TsType(this FieldInfo propertyInfo)
        {
            return propertyInfo.FieldType.TsType();
        }

        public static IEnumerable<FieldInfo> FieldFilter(this IEnumerable<FieldInfo> propertyInfos)
        {
            return propertyInfos.Where(x =>
                !x.FieldType.IsGenericType ||
                x.FieldType.IsGenericType && x.FieldType.GetGenericTypeDefinition() != typeof(IObservable<>));
        }

        public static object TryGetConstant(this FieldInfo fieldInfo)
        {
            var dictionary = fieldInfo.DeclaringType.GetConstants();
            var key = dictionary.Keys.FirstOrDefault(x => x == fieldInfo);
            return key == null ? null : dictionary[key];
        }
    }
}