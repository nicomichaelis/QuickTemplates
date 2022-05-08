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

        var fileHead = BuildFileHead(meta, template).ToList().AsReadOnly();
        var fileContent = new List<ModelNode>().AsReadOnly();
        var fileBottom = BuildFileBottom(meta, template).ToList().AsReadOnly();

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
            yield return new FixedLineNode(line.Text, line.Indented);
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
            yield return new FixedLineNode(line.Text, line.Indented);
        }
    }

    IEnumerable<ModelNode> BuildFileBottom(List<MetaData> meta, Template template)
    {
        foreach (var line in meta.OfType<Line>().Where(z => z.Position == LinePostition.Bottom))
        {
            yield return new FixedLineNode(line.Text, line.Indented);
        }
   }
}