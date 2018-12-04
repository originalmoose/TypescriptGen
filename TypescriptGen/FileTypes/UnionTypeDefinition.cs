using System.Collections.Generic;
using System.Linq;
using TypescriptGen.Helpers;

namespace TypescriptGen.FileTypes
{
    public class UnionTypeDefinition : TsFile
    {
        public UnionTypeDefinition(string export, TsDir directory) : base(export, directory)
        {
            Export = export;
        }

        public List<TypedFile> TypesForUnion { get; } = new List<TypedFile>();

        public override string ToString()
        {
            if (!TypesForUnion.Any())
                return "";

            var builder = new IndentedStringBuilder();

            foreach (var type in TypesForUnion) builder.AppendLine(type.Import(Directory));

            builder.AppendLine()
                .AppendLine($"export type {Export} = {string.Join(" | ", TypesForUnion.Select(t => t.Export))};");

            return builder;
        }

        public static implicit operator string(UnionTypeDefinition union)
        {
            return union.ToString();
        }
    }
}