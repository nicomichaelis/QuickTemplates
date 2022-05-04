using Michaelis.QuickTemplates.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Michaelis.QuickTemplates.Meta;

class MetaReader
{
    Dictionary<string, Type> availableTypes;

    static readonly Regex metaRegex = new Regex(@"^\s*(?<type>\w+)(\s+(?<property>\w+)\s*=\s*(?<string>@""(?:[^""]|"""")*""|""(?:\\""|[^\\""])*""))*\s*$", RegexOptions.ExplicitCapture);

    public MetaReader(params Type[] availableTypesList)
    {
        availableTypes = availableTypesList.ToDictionary(z => z.Name, StringComparer.OrdinalIgnoreCase);
    }

    public MetaReader() : this(typeof(Template), typeof(Import), typeof(Parameter), typeof(Line))
    {
    }

    public List<MetaData> DecodeMeta(List<TemplateDirective> directives, DiagnosticsCollection diagnostics)
    {
        List<MetaData> result = new();
        foreach (var directive in directives)
        {
            if (directive.Mode != DirectiveMode.Meta) continue;
            var meta = Decode(directive, diagnostics);
            if (null != meta)
            {
                result.Add(meta);
            }
        }
        return result;
    }

    private MetaData Decode(TemplateDirective directive, DiagnosticsCollection diagnostics)
    {
        var text = directive.Data;
        var m = metaRegex.Match(text);
        if (!m.Success)
        {
            diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.MalformedDirective(directive.Location)));
            return null;
        }
        var typeString = m.Groups["type"].Value;
        if (!availableTypes.TryGetValue(typeString, out var type))
        {
            diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.DirectiveUnknown(directive.Location, typeString)));
            return null;
        }

        MetaData obj = (MetaData)System.Activator.CreateInstance(type);
        obj.Directive = directive;
        var propertyGroup = m.Groups["property"];
        var stringGroup = m.Groups["string"];
        for (int i = 0; i < propertyGroup.Captures.Count; i++)
        {
            string property = propertyGroup.Captures[i].Value;
            string str = DecodeString(stringGroup.Captures[i].Value);
            var propInfo = type.GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (propInfo == null)
            {
                diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.PropertyUnknown(directive.Location, property)));
                return null;
            }
            object value;
            try
            {
                value = GetValue(str, propInfo.PropertyType);
            }
            catch
            {
                diagnostics.Add(new(DiagnosticSeverity.Error, DiagnosticMessages.MalformedValue(directive.Location, str)));
                return null;
            }
            propInfo.SetValue(obj, value);
        }
        return obj;
    }

    private object GetValue(string str, Type type)
    {
        if (type == typeof(string))
        {
            return str;
        }
        else if (type == typeof(int))
        {
            return int.Parse(str);
        }
        else if (type == typeof(long))
        {
            return long.Parse(str);
        }
        else if (type == typeof(bool))
        {
            return bool.Parse(str);
        }
        else if (type.IsEnum)
        {
            return Enum.Parse(type, str, true);
        }
        else throw new NotImplementedException(type.FullName);
    }

    private string DecodeString(string value)
    {
        var builder = StringUtils.AcquireStringBuilder();
        if (value.StartsWith("@\""))
        {
            for (int i = 2; i < value.Length - 1; i++)
            {
                if (value[i] == '"') // " only appears at beginning/end or is doubled acc. to regex
                {
                    builder.Append('"');
                    i++;
                }
                else builder.Append(value[i]);
            }
        }
        else
        {
            for (int i = 1; i < value.Length - 1; i++)
            {
                if (value[i] == '\\') // \ always followed by " meaning escape according to regex
                {
                    builder.Append('"');
                    i++;
                }
                else builder.Append(value[i]);
            }
        }
        return StringUtils.GetStringAndRelease(builder);
    }
}
