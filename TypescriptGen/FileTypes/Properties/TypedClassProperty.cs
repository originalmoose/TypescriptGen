using System.Reflection;

namespace TypescriptGen.FileTypes.Properties
{
    public class TypedClassProperty : ClassProperty
    {
        public TypedClassProperty(TypeBuilder builder, PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            Name = PropertyInfo.Name;
            Type = PropertyInfo.TsType();

            var isExternalType = false;
            var useTicks = false;

            switch (Type.Replace("Array<", "").Replace(">", "").Replace(" | null", ""))
            {
                case "string":
                    useTicks = true;
                    break;
                case "boolean":
                case "number":
                case "Date":
                case "void":
                case "any":
                    break;
                default:
                    isExternalType = true;
                    break;
            }

            var constValue = propertyInfo.TryGetConstant();

            if (constValue != null)
            {
                Type = $"{(useTicks ? TypeBuilder.TickStile : "")}{constValue}{(useTicks ? TypeBuilder.TickStile : "")}";
                return;
            }

            if (isExternalType) Dependencies.Add(builder.Type(propertyInfo.PropertyType));
        }

        public PropertyInfo PropertyInfo { get; }
    }
}