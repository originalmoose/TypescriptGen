namespace TypescriptGen.FileTypes.Properties
{
    public class InterfaceProperty : Property
    {
        public InterfaceProperty(string name, string type, bool isOptional = false)
        {
            Name = name;
            Type = type;
            IsOptional = isOptional;
        }
    }
}