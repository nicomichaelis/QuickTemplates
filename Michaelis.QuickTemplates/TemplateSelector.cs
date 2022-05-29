using System;
using Michaelis.QuickTemplates.CsTemplates;

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
            FileNode fileNode =>
                context =>
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
            FixedLineNode line => context => new FixedLineTemplate() { Context = context }.TransformText(this, line),
            LineInfoNode line => context => new LineInfoTemplate() { Context = context }.TransformText(this, line),
            LineEndInfoNode line => context => new LineEndInfoTemplate() { Context = context }.TransformText(this, line),
            ClassNode cls => context => new ClassTemplate() { Context = context }.TransformText(this, cls),
            ContextClassCodeNode cccn => context => new ContextClassCodeTemplate() { Context = context }.TransformText(this, cccn),
            BaseClassCodeNode bccn => context => new BaseClassCodeTemplate() { Context = context }.TransformText(this, bccn),
            SimplePropertyNode spn => context => new SimplePropertyTemplate() { Context = context }.TransformText(this, spn),
            MethodNode mn => context => new MethodTemplate() { Context = context }.TransformText(this, mn),
            ParameterNode pn => context => new ParameterTemplate() { Context = context }.TransformText(this, pn),
            _ => throw new NotSupportedException()
        };
    }
}