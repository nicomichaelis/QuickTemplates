namespace Michaelis.QuickTemplates.Meta;

class Template : MetaData
{
    public string Visibility { get; set; } = "public";
    public string Name { get; set; } = "";
    public string Namespace { get; set; } = "";
    public string Inherits { get; set; } = "";
    public bool Linepragmas { get; set; } = true;
    public string TransformMethod { get; set; } = "TransformText";
    public bool OmitGeneratedAttribute { get; set; } = false;
    public string TransformMethodVisibility { get; set; } = "public";
    public string TransformMethodAttribute { get; set; } = "";

    public string TemplateName => (string.IsNullOrEmpty(Name) ? System.IO.Path.GetFileNameWithoutExtension(Directive.Location.SourceName) : Name);
}
