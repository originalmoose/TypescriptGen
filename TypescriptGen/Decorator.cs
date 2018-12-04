using TypeGen.FileTypes;

namespace TypeGen
{
    public class Decorator
    {
        public string Output { get; set; }

        public TsFile Dependency { get; set; }

        public StaticDependency StaticDependency { get; set; }

        public override string ToString() => Output.StartsWith("@") ? Output : $"@{Output}";

        public static implicit operator string(Decorator decorator) => decorator.ToString();
    }
}