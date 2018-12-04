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
        public InterfaceFile(TypeBuilder builder, Type type, TsDir rootDir) : base(type, rootDir)
        {
            Properties.AddRange(type.GetProperties().PropertyFilter()
                .Select(prop => new TypedInterfaceProperty(builder, prop)));
        }

        public List<Property> Properties { get; } = new List<Property>();

        public override string ToString()
        {
            var builder = new IndentedStringBuilder();

            WriteDependencies(builder, Properties);

            builder.AppendLine($"export interface {Export}");
            builder.AppendLine("{");

            using (builder.Indent())
            {
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