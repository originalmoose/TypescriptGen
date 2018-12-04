using System.Reflection;

namespace TypescriptGen.FileTypes.Properties
{
    public class TypedInterfaceProperty : InterfaceProperty
    {
        public TypedInterfaceProperty(TypeBuilder builder, PropertyInfo propertyInfo) : base(propertyInfo.Name, propertyInfo.TsType())
        {
            PropertyInfo = propertyInfo;

            var isExternalType = false;
            var useTicks = false;

            switch (Type)
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