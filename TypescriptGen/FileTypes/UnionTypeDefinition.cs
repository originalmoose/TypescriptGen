using System.Collections.Generic;
using System.Linq;
using TypeGen.Helpers;

namespace TypeGen.FileTypes
{
    public class UnionTypeDefinition : TsFile
    {
        public List<TypedFile> TypesForUnion { get; } = new List<TypedFile>();

        public UnionTypeDefinition(string export, TsDir directory) : base(export, directory)
        {
            Export = export;
        }

        public override string ToString()
        {
            if (!TypesForUnion.Any())
                return "";

            var builder = new IndentedStringBuilder();

            foreach (var type in TypesForUnion)
            {
                builder.AppendLine(type.Import(Directory));
            }

            builder.AppendLine()
                .AppendLine($"export type {Export} = {string.Join(" | ", TypesForUnion.Select(t => t.Export))};");

            return builder;
        }

        public static implicit operator string(UnionTypeDefinition union) => union.ToString();
    }
}