using System;
using System.Reflection;

namespace TypescriptGen.FileTypes.Properties
{
    public class TypedFieldProperty : ClassProperty
    {
        public TypedFieldProperty(TypeBuilder builder, FieldInfo fieldInfo)
        {
            FieldInfo = fieldInfo;
            Name = FieldInfo.Name;
            Type = FieldInfo.FieldType.TsType();

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

            var constValue = fieldInfo.TryGetConstant();

            if (constValue != null)
            {
                Type = $"{(useTicks ? TypeBuilder.TickStile : "")}{constValue}{(useTicks ? TypeBuilder.TickStile : "")}";
                return;
            }

            if (isExternalType) Dependencies.Add(builder.Type(fieldInfo.FieldType));
        }

        public FieldInfo FieldInfo { get; }
    }
}