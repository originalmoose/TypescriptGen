using System;
using System.Collections.Generic;
using System.Linq;
using TypescriptGen.FileTypes.Properties;
using TypescriptGen.Helpers;

namespace TypescriptGen.FileTypes
{
    public class EnumFile : TypedFile
    {
        public EnumFile(Type type, TsDir rootDir) : base(type, rootDir)
        {
            Properties.AddRange(Enum.GetNames(type).Select(name =>
            {
                return new EnumProperty
                {
                    Name = name,
                    Value = ((int) Enum.Parse(type, name)).ToString()
                };
            }));
        }

        public List<EnumProperty> Properties { get; } = new List<EnumProperty>();

        public override string ToString()
        {
            var builder = new IndentedStringBuilder();

            builder.AppendLine($"export enum {Export}");
            builder.AppendLine("{");

            using (builder.Indent())
            {
                foreach (var enumProperty in Properties)
                    builder.AppendLine(enumProperty);
            }

            builder.AppendLine("}");

            return builder;
        }

        public static implicit operator string(EnumFile file)
        {
            return file.ToString();
        }
    }
}