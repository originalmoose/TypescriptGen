using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypescriptGen.FileTypes.Properties;
using TypescriptGen.Helpers;

namespace TypescriptGen.FileTypes
{
    public class ClassFile : TypedFile
    {
        public ClassFile(TypeBuilder builder, Type type, TsDir rootDir) : base(type, rootDir)
        {
            Properties.AddRange(type.GetProperties().PropertyFilter()
                .Select(prop =>
                {
                    var tsProp = new TypedClassProperty(builder, prop);

                    foreach (var decorator in builder.DefaultClassPropertyDecorators) tsProp.Decorators.Add(decorator);

                    return tsProp;
                }));

            Fields.AddRange(type.GetFields().FieldFilter().Select(field => new TypedFieldProperty(builder, field)));
        }

        public List<Property> Properties { get; } = new List<Property>();
        public List<TypedFieldProperty> Fields { get; } = new List<TypedFieldProperty>();

        public override string ToString()
        {
            var builder = new IndentedStringBuilder();

            WriteDependencies(builder, Properties);

            builder.AppendLine($"export class {Export} {{");

            using (builder.Indent())
            {
                foreach (var field in Fields)
                {
                    builder.AppendLine(field);
                    if (TypeBuilder.LineBetweenProperties)
                        builder.AppendLine();
                }

                foreach (var classProperty in Properties)
                {
                    builder.AppendLine(classProperty);
                    if (TypeBuilder.LineBetweenProperties)
                        builder.AppendLine();
                }
            }

            builder.AppendLine("}");
            return builder;
        }

        public static implicit operator string(ClassFile classFile)
        {
            return classFile.ToString();
        }
    }
}