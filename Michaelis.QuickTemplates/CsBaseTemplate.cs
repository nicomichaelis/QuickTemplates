namespace Michaelis.QuickTemplates.CsTemplates;

abstract partial class CsBaseTemplate
{
    protected void ApplyNode(CsBaseTemplateContext context, TemplateSelector selector, ModelNode node)
    {
        var transformText = selector.GetTemplateTransformForNode(node);
        transformText(context);
    }

    protected string GetIf(string joint, bool a, bool b)
    {
        return (a && b) ? joint : string.Empty;
    }

    protected string GetIf(string joint, bool a)
    {
        return a ? joint : string.Empty;
    }

    protected string GetIf(string joint, string a, string b)
    {
        return (!(string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b))) ? joint : string.Empty;
    }

    protected string GetIf(string joint, string a)
    {
        return (!(string.IsNullOrEmpty(a))) ? joint : string.Empty;
    }
}
