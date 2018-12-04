using System.Collections.Generic;
using System.Text;

namespace TypeGen.FileTypes.Properties
{
    public class ClassProperty : Property
    {
        public List<Decorator> Decorators { get; } = new List<Decorator>();

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var decorator in Decorators)
            {
                builder.AppendLine(decorator);
            }

            builder.Append($"{Name}{(IsOptional ? "?" : "")}: {Type};");

            return builder.ToString();
        }

        public static implicit operator string(ClassProperty property) => property.ToString();
    }
}