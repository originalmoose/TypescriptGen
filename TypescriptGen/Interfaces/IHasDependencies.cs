using System.Collections.Generic;
using TypeGen.FileTypes;

namespace TypeGen.Interfaces
{
    public interface IHasDependencies
    {
        List<TsFile> Dependencies { get; }

        List<StaticDependency> StaticDependencies { get; }
    }
}