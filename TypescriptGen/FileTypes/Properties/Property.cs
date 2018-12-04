using System.Collections.Generic;
using TypescriptGen.Interfaces;

namespace TypescriptGen.FileTypes.Properties
{
    public class Property : IHasDependencies
    {
        /// <summary>
        ///     The name of the property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The typescript type for the property
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Optional properties will be marked with ?
        /// </summary>
        public bool IsOptional { get; set; }

        /// <summary>
        ///     A list of dependencies for this property. If the type for the property needs to be imported those imports go here.
        /// </summary>
        public List<TsFile> Dependencies { get; } = new List<TsFile>();

        /// <summary>
        ///     A list of static dependencies ie import { SomeExport } from 'some-library';
        /// </summary>
        public List<StaticDependency> StaticDependencies { get; } = new List<StaticDependency>();

        public override string ToString()
        {
            return $"{Name}{(IsOptional ? "?" : "")}: {Type};";
        }

        public static implicit operator string(Property prop)
        {
            return prop.ToString();
        }
    }
}