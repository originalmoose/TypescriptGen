using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                var stringValue = "";
                var val =  (Enum.Parse(type, name));
                var t = val.GetType().GetEnumUnderlyingType();
                if(t.FullName == typeof(int).FullName)
                    stringValue = ((int)val).ToString();
                else if(t.FullName == typeof(byte).FullName)
                    stringValue = ((byte)val).ToString();
                else
                {
                    Console.WriteLine("Unknown enum underlying type");
                }
                return new EnumProperty
                {
                    Name = name,
                    Value = stringValue,
                };
            }));
        }

        public List<EnumProperty> Properties { get; } = new List<EnumProperty>();

        public override string ToString()
        {
            var builder = new IndentedStringBuilder();

            builder.AppendLine($"export enum {Export} {{");

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