namespace Michaelis.QuickTemplates.Meta;

class Parameter : MetaData
{
    public string Type { get; set; }
    public ParameterAvailability Availability { get; set; } = ParameterAvailability.Method;
    public string Name { get; set; }
    public string Initializer { get; set; }
    public bool Required { get; set; } = false;
}
