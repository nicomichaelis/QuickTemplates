namespace Michaelis.QuickTemplates.Meta;

class Template : MetaData
{
    public TemplateVisibility Visibility { get; set; } = TemplateVisibility.@public;
    public string Name { get; set; }
    public string Namespace { get; set; }
    public string Inherits { get; set; } = null;
    public bool Linepragmas { get; set; } = true;
    public string TransformMethod { get; set; } = "TransformText";
    public bool OmitGeneratedAttribute { get; set; } = false;
    public TemplateVisibility TransformMethodVisibility { get; set; } = TemplateVisibility.@public;
    public string TransformMethodAttribute { get; set; } = null;
}
