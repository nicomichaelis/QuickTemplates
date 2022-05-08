namespace Michaelis.QuickTemplates.CsTemplates;

abstract partial class CsBaseTemplate
{
    protected void ApplyNode(CsBaseTemplateContext context, TemplateSelector selector, ModelNode node)
    {
        var transformText = selector.GetTemplateTransformForNode(node);
        transformText(context);
    }
}
