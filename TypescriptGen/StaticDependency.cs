using System.Collections.Generic;
using System.Linq;

namespace TypescriptGen
{
    public class StaticDependency
    {
        /// <summary>
        ///     Use this if you want to use a default import
        /// </summary>
        public string DefaultExport { get; set; }

        /// <summary>
        ///     Set this to true if you want to have import * as <see cref="DefaultExport" /> instead of import
        ///     <see cref="DefaultExport" />
        /// </summary>
        public bool UseStarAs { get; set; }

        public List<string> Exports { get; } = new List<string>();
        public string ImportPath { get; set; }

        public override string ToString()
        {
            var from = $"from {TypeBuilder.TickStile}{ImportPath}{TypeBuilder.TickStile}";
            var exportList = $"{{ {string.Join(", ", Exports)} }}";
            if (string.IsNullOrEmpty(DefaultExport))
                return $"import {exportList} {from};";

            var defaultExport = $"{(UseStarAs ? "* as " : "")}{DefaultExport}";
            if (Exports.Any()) return $"import {defaultExport}, {exportList} {from};";

            return $"import {defaultExport} {from};";
        }

        public static implicit operator string(StaticDependency staticDependency)
        {
            return staticDependency.ToString();
        }
    }
}