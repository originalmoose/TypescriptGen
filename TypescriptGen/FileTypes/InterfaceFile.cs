using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypescriptGen.FileTypes.Properties;
using TypescriptGen.Helpers;

namespace TypescriptGen.FileTypes
{
    public class InterfaceFile : TypedFile
    {
        public InterfaceFile(TypeBuilder builder, Type type, TsDir rootDir, bool forceInterfaceForProperties = false) : base(type, rootDir)
        {
            if(!Export.StartsWith("I"))
                Export = $"I{Export}";
            Properties.AddRange(type.GetProperties().PropertyFilter()
                .Select(prop => new TypedInterfaceProperty(builder, prop, forceInterfaceForProperties)));
            Fields.AddRange(type.GetFields().FieldFilter().Select(field => new TypedFieldProperty(builder, field)));
        }

        public List<Property> Properties { get; } = new List<Property>();
        public List<TypedFieldProperty> Fields { get; } = new List<TypedFieldProperty>();

        public override string ToString()
        {
            var builder = new IndentedStringBuilder();

            WriteDependencies(builder, Properties);

            builder.AppendLine($"export interface {Export} {{");

            using (builder.Indent())
            {
                foreach (var field in Fields)
                {
                    builder.AppendLine(field);
                    if (TypeBuilder.LineBetweenProperties)
                        builder.AppendLine();
                }

                foreach (var interfaceProperty in Properties)
                {
                    builder.AppendLine(interfaceProperty);
                    if (TypeBuilder.LineBetweenProperties)
                        builder.AppendLine();
                }
            }

            builder.AppendLine("}");

            return builder;
        }

        public static implicit operator string(InterfaceFile file)
        {
            return file.ToString();
        }
    }
}