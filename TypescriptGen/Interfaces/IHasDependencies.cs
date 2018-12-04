using System.Collections.Generic;
using TypescriptGen.FileTypes;

namespace TypescriptGen.Interfaces
{
    public interface IHasDependencies
    {
        List<TsFile> Dependencies { get; }

        List<StaticDependency> StaticDependencies { get; }
    }
}