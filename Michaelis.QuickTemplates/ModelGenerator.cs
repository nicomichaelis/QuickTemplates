using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Michaelis.QuickTemplates.Meta;

namespace Michaelis.QuickTemplates;

internal class ModelGenerator
{
    private enum ContentType { Context, TemplateBase, Template }

    public IEnumerable<FileNode> Generate(InputData input, List<MetaData> meta, List<TemplateDirective> directives)
    {
        var template = meta.OfType<Template>().Last();
        if (string.IsNullOrEmpty(template.Inherits))
        {
            yield return BuildFile(input, meta, directives, template, ContentType.Context);
            yield return BuildFile(input, meta, directives, template, ContentType.TemplateBase);
        }
        yield return BuildFile(input, meta, directives, template, ContentType.Template);
    }

    private FileNode BuildFile(InputData input, List<MetaData> meta, List<TemplateDirective> directives, Template template, ContentType content)
    {
        var fileHead = BuildFileHead(meta, template).ToList().AsReadOnly();
        var fileContent = BuildFileContent(meta, template, directives, content).ToList().AsReadOnly();
        var fileBottom = BuildFileBottom(meta, template).ToList().AsReadOnly();
        var postfix = ContentTypeToName(content);
        FileNode node = new FileNode(
            Path.Combine(Path.GetDirectoryName(input.SourceRelativeLocation), Path.GetFileNameWithoutExtension(input.SourceRelativeLocation) + postfix + ".cs"),
            template.Namespace,
            fileHead, fileContent, fileBottom
        );
        return node;
    }

    private static string ContentTypeToName(ContentType content) => content switch
    {
        ContentType.Template => "",
        ContentType.Context => "Context",
        ContentType.TemplateBase => "Base",
        _ => throw new NotImplementedException()
    };

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

    IEnumerable<ModelNode> BuildFileContent(List<MetaData> meta, Template template, List<TemplateDirective> directives, ContentType content)
    {
        string origClassname = (template.Name ?? Path.GetFileNameWithoutExtension(template.Directive.Location.SourceName));

        var classHead = new List<ModelNode>().AsReadOnly();
        List<ModelNode> classContent = content switch
        {
            ContentType.TemplateBase => new() { new BaseClassCodeNode(origClassname) },
            ContentType.Context => new() { new ContextClassCodeNode() },
            ContentType.Template => BuildTemplateClassContent(meta, template, directives).ToList(),
            _ => throw new NotImplementedException()
        };

        var classBottom = new List<ModelNode>().AsReadOnly();

        var cls = new ClassNode(origClassname + ContentTypeToName(content), template.Visibility, classHead, classContent.AsReadOnly(), classBottom);
        yield return cls;
    }

    private IEnumerable<ModelNode> BuildTemplateClassContent(List<MetaData> meta, Template template, List<TemplateDirective> directives)
    {
        var memberParameters = meta.OfType<Parameter>().Where(z => z.Availability == ParameterAvailability.Class).ToList();
        foreach (var z in memberParameters)
        {
            yield return new SimplePropertyNode(z.Name, z.Type, z.Modifier, z.GetAccessor, z.SetAccessor, z.Initializer);
        }
    }
}