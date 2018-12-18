using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class TypeExtensions
    {
        public static string TsType(this Type type)
        {
            while (true)
            {
                var isArray = type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>) || type.GetGenericTypeDefinition() == typeof(IList<>));

                if (isArray) return $"Array<{type.GenericTypeArguments[0].TsType()}>";

                return type.TsTypeInternal();
            }
        }

        private static string TsTypeInternal(this Type type)
        {
            var isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            var arrayType = type.IsArray ? type.GetElementType() : null;
            var fullName = isNullable ? type.GenericTypeArguments[0].FullName : (arrayType != null ? arrayType.FullName : type.FullName);

            var t = isNullable ? type.GenericTypeArguments[0].Name : type.Name;

            switch (fullName)
            {
                case "System.Boolean":
                    t = $"boolean{(arrayType == null ? "" : "[]")}";
                    break;
                case "System.String":
                case "System.Char":
                case "System.Guid":
                case "System.TimeSpan":
                    t = $"string{(arrayType == null ? "" : "[]")}";
                    break;
                case "System.Byte":
                case "System.SByte":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                case "System.Single":
                case "System.Double":
                case "System.Decimal":
                    t = $"number{(arrayType == null ? "" : "[]")}";
                    break;
                case "System.DateTime":
                case "System.DateTimeOffset":
                    t = $"Date{(arrayType == null ? "" : "[]")}";
                    break;
                case "System.Void":
                    t = "void";
                    break;
                case "System.Object":
                case "dynamic":
                    t = $"any{(arrayType == null ? "" : "[]")}";
                    break;
                default:
                    break;
            }

            return $"{t}{(isNullable ? " | null" : "")}";
        }

        public static Dictionary<FieldInfo, object> GetConstants(this Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(x => x.IsLiteral && !x.IsInitOnly)
                .ToDictionary(x => x, x => x.GetRawConstantValue());
        }

        public static Type UnderlyingType(this Type type)
        {
            var t = type;
            while (t.IsArray || (t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(IEnumerable<>) || t.GetGenericTypeDefinition() == typeof(IList<>))))
            {
                if (t.IsArray)
                {
                    t = type.GetElementType() ?? throw new NullReferenceException("Unable to get Array ElementType");
                    continue;
                }

                if (t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(IEnumerable<>) || t.GetGenericTypeDefinition() == typeof(IList<>)))
                {
                    t = type.GenericTypeArguments[0];
                    continue;
                }
            }

            return t;
        }
    }
}