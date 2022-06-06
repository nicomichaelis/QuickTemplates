using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Michaelis.QuickTemplates.Meta;
using Michaelis.QuickTemplates.Text;

namespace Michaelis.QuickTemplates;

internal class ModelGenerator
{
    enum ContentType { Context, TemplateBase, Template }

    public IEnumerable<FileNode> Generate(InputData input, List<MetaData> meta, List<TemplateDirective> directives, Dictionary<string, TemplateDirectiveAndMetaData> inheritanceMeta)
    {
        var template = meta.OfType<Template>().Last();
        if (string.IsNullOrEmpty(template.Inherits))
        {
            yield return BuildFile(input, meta, directives, template, ContentType.Context, inheritanceMeta);
            yield return BuildFile(input, meta, directives, template, ContentType.TemplateBase, inheritanceMeta);
        }
        yield return BuildFile(input, meta, directives, template, ContentType.Template, inheritanceMeta);
    }

    FileNode BuildFile(InputData input, List<MetaData> meta, List<TemplateDirective> directives, Template template, ContentType content, Dictionary<string, TemplateDirectiveAndMetaData> inheritanceMeta)
    {
        var fileHead = BuildFileHead(meta, template).ToList().AsReadOnly();
        var fileContent = BuildFileContent(meta, template, directives, content, inheritanceMeta).ToList().AsReadOnly();
        var fileBottom = BuildFileBottom(meta, template).ToList().AsReadOnly();
        var postfix = ContentTypeToName(content);
        FileNode node = new FileNode(
            Path.Combine(Path.GetDirectoryName(input.SourceRelativeLocation), Path.GetFileNameWithoutExtension(input.SourceRelativeLocation) + postfix + ".cs"),
            template.Namespace,
            fileHead, fileContent, fileBottom
        );
        return node;
    }

    static string ContentTypeToName(ContentType content) => content switch
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

    IEnumerable<ModelNode> BuildFileContent(List<MetaData> meta, Template template, List<TemplateDirective> directives, ContentType content, Dictionary<string, TemplateDirectiveAndMetaData> inheritanceMeta)
    {
        string origClassname = template.TemplateName;

        var classHead = new List<ModelNode>();
        if (!template.OmitGeneratedAttribute) classHead.Add(new FixedLineNode($"[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"{ThisAssembly.AssemblyTitle}\", \"{ThisAssembly.AssemblyVersion}\")]", true));

        List<ModelNode> classContent = content switch
        {
            ContentType.TemplateBase => new() { new BaseClassCodeNode(origClassname) },
            ContentType.Context => new() { new ContextClassCodeNode() },
            ContentType.Template => BuildTemplateClassContent(meta, template, directives, inheritanceMeta).ToList(),
            _ => throw new NotImplementedException()
        };

        var classBottom = new List<ModelNode>().AsReadOnly();

        var cls = new ClassNode(origClassname + ContentTypeToName(content), template.Visibility, InheritsFrom(origClassname, content, template), classHead.AsReadOnly(), classContent.AsReadOnly(), classBottom);
        yield return cls;
    }

    string InheritsFrom(string origClassname, ContentType content, Template template)
    {
        if (content != ContentType.Template) return "";
        if (!string.IsNullOrEmpty(template.Inherits))
        {
            return template.Inherits;
        }
        else
        {
            return origClassname + ContentTypeToName(ContentType.TemplateBase);
        }
    }

    IEnumerable<ModelNode> BuildTemplateClassContent(List<MetaData> meta, Template template, List<TemplateDirective> directives, Dictionary<string, TemplateDirectiveAndMetaData> inheritanceMeta)
    {
        var memberParameters = meta.OfType<Parameter>().Where(z => z.Availability == ParameterAvailability.Class).ToList();
        foreach (var z in memberParameters)
        {
            yield return new SimplePropertyNode(z.Name, z.Type, z.Modifier, z.GetAccessor, z.SetAccessor, z.Initializer);
        }

        bool hasLineInfo = false;
        foreach (var dir in directives.Where(z => z.Mode == DirectiveMode.ClassCode))
        {
            if (template.Linepragmas)
            {
                yield return new LineInfoNode(dir.Location.SourceName, dir.Location.Line);
                hasLineInfo = true;
            }
            yield return new FixedLineNode(new string(' ', Math.Max(0, dir.Location.Col - 1)) + dir.Data, false);
        }
        if (hasLineInfo)
        {
            yield return new LineEndInfoNode(FinishLineInfoMode.Default);
            hasLineInfo = false;
        }

        var daisychainedMeta = DaisyChainMeta(template, inheritanceMeta);

        if (directives.Any(z => (z.Mode != DirectiveMode.Meta) && (z.Mode != DirectiveMode.ClassCode)))
        {
            List<ModelNode> methodContent = BuildTemplateMethodContent(meta, template, directives);
            List<ModelNode> methodParams =
                daisychainedMeta.OfType<Parameter>()
                .Where(z => z.Availability == ParameterAvailability.Method)
                .Select(parameter => new ParameterNode(parameter.Type, parameter.Name))
                .OfType<ModelNode>().Intersperse((pos) => (true,
                    new FixedLineNode(pos switch
                    {
                        InterspersePosition.Init => "",
                        InterspersePosition.Between => ",",
                        InterspersePosition.End => "",
                        _ => throw new NotSupportedException(pos.ToString())
                    }, true))).ToList();
            List<ModelNode> methdoHead = meta.OfType<Line>()
                .Where(z => z.Position == LinePostition.PreTransformMethod)
                .Select(line => new FixedLineNode(line.Text, line.Indented))
                .OfType<ModelNode>().ToList();

            var method = new MethodNode(template.TransformMethodVisibility, template.TransformMethodAttribute, "void", template.TransformMethod, methodParams.AsReadOnly(),
                Enumerable.Empty<ModelNode>().ToList().AsReadOnly(), methodContent.AsReadOnly());
            yield return method;
        }
    }

    private IEnumerable<MetaData> DaisyChainMeta(Template template, Dictionary<string, TemplateDirectiveAndMetaData> inheritanceMeta)
    {
        IEnumerable<MetaData> par = Enumerable.Empty<MetaData>();
        if (inheritanceMeta.TryGetValue(template.Inherits, out var parData))
        {
            par = DaisyChainMeta(parData.Meta.OfType<Template>().First(), inheritanceMeta);
        }

        return par.Concat(inheritanceMeta[template.TemplateName].Meta);
    }

    List<ModelNode> BuildTemplateMethodContent(List<MetaData> meta, Template template, List<TemplateDirective> directives)
    {
        List<ModelNode> result = new();
        if (template.Linepragmas)
        {
            result.Add(new LineEndInfoNode(FinishLineInfoMode.Hidden));
        }
        foreach (var dir in directives)
        {
            switch (dir.Mode)
            {
                case DirectiveMode.Meta:
                case DirectiveMode.ClassCode:
                    break;
                case DirectiveMode.NewLine:
                    result.Add(new FixedLineNode("WriteLine();", true));
                    break;
                case DirectiveMode.Text:
                    result.Add(new FixedLineNode($"WriteNoBreakIndent({StringUtils.SafeEncodeString(dir.Data)});", true));
                    break;
                case DirectiveMode.Insert:
                    result.Add(new FixedLineNode($"WriteFormated(", true));
                    if (template.Linepragmas)
                    {
                        result.Add(new LineInfoNode(dir.Location.SourceName, dir.Location.Line));
                    }
                    string prefix = template.Linepragmas
                                  ? new string(' ', Math.Max(0, dir.Location.Col - 1 - 4))
                                  : "    ";
                    result.Add(new FixedLineNode($"{prefix}$\"{{({dir.Data})}}\");", !template.Linepragmas));
                    if (template.Linepragmas)
                    {
                        result.Add(new LineEndInfoNode(FinishLineInfoMode.Hidden));
                    }
                    break;
                case DirectiveMode.Code:
                    if (template.Linepragmas)
                    {
                        result.Add(new LineInfoNode(dir.Location.SourceName, dir.Location.Line));
                    }
                    string cprefix = template.Linepragmas
                                  ? new string(' ', Math.Max(0, dir.Location.Col - 1))
                                  : "";
                    result.Add(new FixedLineNode($"{cprefix}{dir.Data}", !template.Linepragmas));
                    if (template.Linepragmas)
                    {
                        result.Add(new LineEndInfoNode(FinishLineInfoMode.Hidden));
                    }
                    break;
                default:
                    throw new NotImplementedException($"{dir.Mode}");
            }
        }

        if (template.Linepragmas)
        {
            result.Add(new LineEndInfoNode(FinishLineInfoMode.Default));
        }
        return result;
    }
}