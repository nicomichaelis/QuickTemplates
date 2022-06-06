namespace Michaelis.QuickTemplates.Meta;

class Parameter : MetaData
{
    public string Type { get; set; }
    public ParameterAvailability Availability { get; set; } = ParameterAvailability.Method;
    public string Name { get; set; }
    public string Initializer { get; set; }
    public string Modifier { get; set; } = "public";
    public string GetAccessor { get; set; } = "get;";
    public string SetAccessor { get; set; } = "set;";
}
