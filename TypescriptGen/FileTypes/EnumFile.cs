﻿using System;
using System.Collections.Generic;
using System.Linq;
using TypeGen.FileTypes.Properties;
using TypeGen.Helpers;

namespace TypeGen.FileTypes
{
    public class EnumFile : TypedFile
    {
        public List<EnumProperty> Properties { get; } = new List<EnumProperty>();

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

        public override string ToString()
        {
            var builder = new IndentedStringBuilder();

            builder.AppendLine($"export enum {Export}");
            builder.AppendLine("{");

            using (builder.Indent())
                foreach (var enumProperty in Properties)
                {
                    builder.AppendLine(enumProperty);
                }

            builder.AppendLine("}");

            return builder;
        }

        public static implicit operator string(EnumFile file) => file.ToString();
    }
}