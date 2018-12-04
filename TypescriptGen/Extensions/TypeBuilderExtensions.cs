using System;
using System.Linq;
using TypeGen.FileTypes;

// ReSharper disable once CheckNamespace
namespace TypeGen
{
    public static class TypeBuilderExtensions
    {
        public static UnionTypeDefinition UnionType<TBaseClass>(this TypeBuilder builder, string nameOverride = null, TsDir directory = null, params Func<Type, bool>[] additionalFilters) 
        {
            var result = builder.UnionType(string.IsNullOrWhiteSpace(nameOverride) ? typeof(TBaseClass).Name : nameOverride, directory, additionalFilters.Union(new Func<Type, bool>[]
            {
                t => t.IsSubclassOf(typeof(TBaseClass))
            }).ToArray());

            return result;
        }
    }
}