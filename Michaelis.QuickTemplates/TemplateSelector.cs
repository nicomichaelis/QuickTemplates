using System;
using Michaelis.QuickTemplates.CsTemplates;
using Michaelis.QuickTemplates.Meta;

namespace Michaelis.QuickTemplates;

class TemplateSelector
{
    int _csLangVersion = 10;

    public TemplateSelector(int csLangVersion)
    {
        _csLangVersion = csLangVersion;
    }

    public Action<CsBaseTemplateContext> GetTemplateTransformForNode(ModelNode node)
    {
        return node switch
        {
            FileNode fileNode => context =>
            {
                if (_csLangVersion >= 10)
                {
                    new CsFileTemplate10() { Context = context }.TransformText(this, fileNode);
                }
                else
                    new CsFileTemplate1() { Context = context }.TransformText(this, fileNode);
            }
            ,
            UsingNode use => context => new UsingTemplate() { Context = context }.TransformText(this, use),
            FixedLineNode line => context => new FixedLineTemplate() { Context = context}.TransformText(this, line),
            LineInfoNode line => context => new LineInfoTemplate() { Context = context}.TransformText(this, line),
            LineEndInfoNode line => context => new LineEndInfoTemplate() { Context = context}.TransformText(this, line),
            _ => throw new NotSupportedException()
        };
    }
}