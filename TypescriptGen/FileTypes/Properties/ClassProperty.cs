using System.Collections.Generic;
using System.Text;

namespace TypescriptGen.FileTypes.Properties
{
    public class ClassProperty : Property
    {
        public List<Decorator> Decorators { get; } = new List<Decorator>();
        public string Initializer {get;set;}

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var decorator in Decorators) builder.AppendLine(decorator);

            var initializer = "";
            if(!string.IsNullOrEmpty(Initializer))
            {
                initializer = Initializer.Trim().StartsWith("=") ? Initializer : $" = {Initializer}";
            }

            builder.Append($"{Name}{(IsOptional ? "?" : "")}: {Type}{initializer};");

            return builder.ToString();
        }

        public static implicit operator string(ClassProperty property)
        {
            return property.ToString();
        }
    }
}