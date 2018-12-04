using System;
using System.Collections.Generic;
using System.Linq;
using TypeGen.Helpers;
using TypeGen.Interfaces;

namespace TypeGen.FileTypes
{
    public abstract class TypedFile : TsFile
    {
        public readonly List<TsFile> Dependencies = new List<TsFile>();
        public readonly List<StaticDependency> StaticDependencies = new List<StaticDependency>();

        public Type Type { get; }

        protected TypedFile(Type type, TsDir rootDir) : base(type.Name,
            type.Namespace == null ? rootDir : type.Namespace.Split('.').Aggregate(rootDir, (current, namespacePart) => current.Down(namespacePart)))
        {
            Type = type;
            Export = type.Name;
        }


        protected void WriteDependencies(IndentedStringBuilder builder, IEnumerable<IHasDependencies> properties)
        {
            var allStaticDependencies = new List<StaticDependency>();

            var hasDependencies = properties as IHasDependencies[] ?? properties.ToArray();
            foreach (var staticDependency in StaticDependencies.Union(hasDependencies.SelectMany(x => x.StaticDependencies)))
            {
                var fromAll = allStaticDependencies.FirstOrDefault(s => s.ImportPath == staticDependency.ImportPath);
                if (fromAll == null)
                {
                    //haven't seen this dependency
                    var x = (new StaticDependency
                    {
                        DefaultExport = staticDependency.DefaultExport,
                        ImportPath = staticDependency.ImportPath,
                        UseStarAs = staticDependency.UseStarAs,
                    });
                    x.Exports.AddRange(staticDependency.Exports);
                    allStaticDependencies.Add(x);
                    continue;
                }

                if (string.IsNullOrEmpty(fromAll.DefaultExport))
                {
                    if (!string.IsNullOrEmpty(staticDependency.DefaultExport))
                        //we can just replace the default export since we don't have one yet
                        fromAll.DefaultExport = staticDependency.DefaultExport;
                }
                else
                {
                    if (!string.IsNullOrEmpty(staticDependency.DefaultExport))
                    {
                        //we have a static dependency that is the same but with a different default export name so it is valid to import
                        allStaticDependencies.Add(new StaticDependency
                        {
                            DefaultExport = staticDependency.DefaultExport,
                            UseStarAs = staticDependency.UseStarAs,
                            ImportPath = staticDependency.ImportPath,
                        });
                    }
                }

                fromAll.Exports.AddRange(staticDependency.Exports.Where(e => fromAll.Exports.Contains(e)));
            }

            foreach (var staticDependency in allStaticDependencies)
            {
                builder.AppendLine(staticDependency);
            }

            foreach (var fileDependency in Dependencies.Union(hasDependencies.SelectMany(x => x.Dependencies)))
            {
                builder.AppendLine(fileDependency.Import(Directory));
            }

            if (Dependencies.Any() || hasDependencies.Any())
                builder.AppendLine();
        }
    }
}