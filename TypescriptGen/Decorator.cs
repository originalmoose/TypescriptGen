using TypescriptGen.FileTypes;

namespace TypescriptGen
{
    public class Decorator
    {
        public string Output { get; set; }

        public TsFile Dependency { get; set; }

        public StaticDependency StaticDependency { get; set; }

        public override string ToString()
        {
            return Output.StartsWith("@") ? Output : $"@{Output}";
        }

        public static implicit operator string(Decorator decorator)
        {
            return decorator.ToString();
        }
    }
}