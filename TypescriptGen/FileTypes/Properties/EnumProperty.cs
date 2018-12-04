namespace TypeGen.FileTypes.Properties
{
    public class EnumProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString() => $"{Name}{(string.IsNullOrEmpty(Value) ? "" : $" = {Value}")},";
        public static implicit operator string(EnumProperty property) => property.ToString();
    }
}