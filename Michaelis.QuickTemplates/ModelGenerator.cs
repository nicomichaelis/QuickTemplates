using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Michaelis.QuickTemplates.Meta;

namespace Michaelis.QuickTemplates;

internal class ModelGenerator
{
    public FileNode Generate(InputData input, List<MetaData> meta, List<TemplateDirective> directives)
    {
        var template = meta.OfType<Template>().Last();

        var fileHead = new List<ModelNode>();
        var fileContent = new List<ModelNode>();
        var fileBottom = new List<ModelNode>();

        fileHead.AddRange(BuildFileHead(meta, template));
        fileBottom.AddRange(BuildFileBottom(meta, template));

        FileNode node = new FileNode(
            Path.ChangeExtension(input.SourceRelativeLocation, ".cs"),
            template.Namespace,
            fileHead, fileContent, fileBottom
        );

        return node;
    }

    IEnumerable<ModelNode> BuildFileHead(List<MetaData> meta, Template template)
    {
        foreach (var line in meta.OfType<Line>().Where(z => z.Position == LinePostition.Head))
        {
            yield return new FixedLineNode(line.Text);
        }

        bool hasLineInfo = false;
        foreach (var import in meta.OfType<Import>().OrderBy(z => z.Namespace, NaturalSortComparer.Default))
        {
            if (template.Linepragmas)
            {
                yield return new LineInfoNode(import.Directive.Location.SourceName, import.Directive.Location.Line);
                hasLineInfo = true;
            }
            yield return new UsingNode(import.Namespace);
        }

        if (hasLineInfo)
        {
            yield return new LineEndInfoNode(FinishLineInfoMode.Default);
        }

        foreach (var line in meta.OfType<Line>().Where(z => z.Position == LinePostition.PreNamespace))
        {
            yield return new FixedLineNode(line.Text);
        }
    }

    IEnumerable<ModelNode> BuildFileBottom(List<MetaData> meta, Template template)
    {
        foreach (var line in meta.OfType<Line>().Where(z => z.Position == LinePostition.Bottom))
        {
            yield return new FixedLineNode(line.Text);
        }
   }

}