using System;
using System.Reflection;

namespace TypescriptGen.FileTypes.Properties
{
    public class TypedInterfaceProperty : InterfaceProperty
    {
        public TypedInterfaceProperty(TypeBuilder builder, PropertyInfo propertyInfo, bool forceInterfaceForProperties = false) : base(propertyInfo.Name, propertyInfo.TsType())
        {
            PropertyInfo = propertyInfo;

            var isExternalType = false;
            var useTicks = false;

            switch (Type.Replace("Array<", "").Replace(">", "").Replace(" | null", "").Replace("[]", ""))
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

            if (isExternalType)
            {
                var t = (forceInterfaceForProperties && !propertyInfo.PropertyType.UnderlyingType().IsEnum ? builder.Interface(propertyInfo.PropertyType, true) : builder.Type(propertyInfo.PropertyType));

                if(t is InterfaceFile interfaceFile)
                {
                    //check the typename against the export
                    if(Type.Replace("Array<", "").Replace(">", "").Replace(" | null", "").Replace("[]", "") != interfaceFile.Export)
                    {
                        Type = Type.Replace(Type.Replace("Array<", "").Replace(">", "").Replace(" | null", "").Replace("[]", ""), interfaceFile.Export);
                    }
                }

                Dependencies.Add(t);

            }
        }

        public PropertyInfo PropertyInfo { get; }
    }
}