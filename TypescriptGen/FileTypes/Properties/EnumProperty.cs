namespace TypescriptGen.FileTypes.Properties
{
    public class EnumProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Name}{(string.IsNullOrEmpty(Value) ? "" : $" = {Value}")},";
        }

        public static implicit operator string(EnumProperty property)
        {
            return property.ToString();
        }
    }
}