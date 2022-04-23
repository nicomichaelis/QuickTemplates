#line 5 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
using Michaelis.QuickTemplates.Meta;
#line 3 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
using System;
#line 2 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
using System.Collections.Generic;
#line 4 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
using System.Linq;
#line default

namespace Michaelis.QuickTemplates
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Michaelis.QuickTemplates", "0.0.0.0")] // 0.0.0+2d233cbacd
    internal partial class TemplateFile : BaseTemplate
    {
        private string _priv_Namespace { get; set; }
        private bool _priv_set_Namespace { get; set; }
        public string Namespace
        {
            get { return _priv_Namespace; }
            set { _priv_Namespace = value; _priv_set_Namespace = true; }
        }

        private TemplateVisibility _priv_Modifier { get; set; }
        private bool _priv_set_Modifier { get; set; }
        public TemplateVisibility Modifier
        {
            get { return _priv_Modifier; }
            set { _priv_Modifier = value; _priv_set_Modifier = true; }
        }

        private string _priv_ClassName { get; set; }
        private bool _priv_set_ClassName { get; set; }
        public string ClassName
        {
            get { return _priv_ClassName; }
            set { _priv_ClassName = value; _priv_set_ClassName = true; }
        }

        private List<MetaData> _priv_Meta { get; set; }
        private bool _priv_set_Meta { get; set; }
        public List<MetaData> Meta
        {
            get { return _priv_Meta; }
            set { _priv_Meta = value; _priv_set_Meta = true; }
        }


        public void TransformText(List<TemplateDirective> directives)
        {
            if (null == directives) throw new ArgumentNullException(nameof(directives));
            if (!_priv_set_Namespace) throw new InvalidOperationException($"{nameof(Namespace)} was not set");
            if (!_priv_set_Modifier) throw new InvalidOperationException($"{nameof(Modifier)} was not set");
            if (!_priv_set_ClassName) throw new InvalidOperationException($"{nameof(ClassName)} was not set");
            if (!_priv_set_Meta) throw new InvalidOperationException($"{nameof(Meta)} was not set");

#line hidden
#line 12 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
            bool any = false;
            var template = Meta.OfType<Template>().Last();
            foreach (var import in Meta.OfType<Import>().OrderBy(z => z.Namespace, NaturalSortComparer.Default))
            {
                any = true;
                StartLineInfo(template, import.Directive);
                WriteLine($"using {import.Namespace};");
            }
            if (any)
            {
                SkipIndent();
                FinishLineInfo(template, FinishLineInfoMode.Default);
            }
#line hidden
            WriteLine();
            WriteNoBreakIndent("namespace ");
            WriteFormated(
#line 26 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
           $"{(Namespace)}");
#line hidden
            WriteLine();
            WriteNoBreakIndent("{");
#line 27 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
            if (!template.OmitGeneratedAttribute) {
#line hidden
            WriteLine();
            WriteNoBreakIndent("    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"");
            WriteFormated(
#line 28 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
                                                              $"{(ThisAssembly.AssemblyTitle)}");
#line hidden
            WriteNoBreakIndent("\", \"");
            WriteFormated(
#line 28 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
                                                                                                   $"{(ThisAssembly.AssemblyVersion)}");
#line hidden
            WriteNoBreakIndent("\")] // ");
            WriteFormated(
#line 28 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
                                                                                                                                             $"{(ThisAssembly.AssemblyInformationalVersion)}");
#line hidden
#line 29 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
            }
#line hidden
            WriteLine();
            WriteNoBreakIndent("    ");
            WriteFormated(
#line 30 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
     $"{(Modifier != TemplateVisibility.none ? Modifier.ToString() + " " : "")}");
#line hidden
            WriteNoBreakIndent("partial class ");
            WriteFormated(
#line 30 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
                                                                                              $"{(template.Name ?? ClassName)}");
#line hidden
            WriteNoBreakIndent(" : ");
            WriteFormated(
#line 30 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
                                                                                                                                  $"{(InheritsFrom(template))}");
#line hidden
            WriteLine();
            WriteNoBreakIndent("    {");
            WriteLine();
#line 32 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
            PushIndent("        ");
            PrintMemberParameters(Meta);

            if (directives.Any(z => (z.Mode != DirectiveMode.Meta) && (z.Mode != DirectiveMode.ClassCode))) {
                GenerateMethodTemplate(directives, template);
            }
            PopIndent();
#line hidden
            WriteNoBreakIndent("    }");
            WriteLine();
#line 41 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
            if (!IsInherited(template)) {
                if (!template.OmitGeneratedAttribute) {
#line hidden
            WriteLine();
            WriteNoBreakIndent("    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"");
            WriteFormated(
#line 43 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
                                                              $"{(typeof(TemplateFile).Assembly.GetName().Name)}");
#line hidden
            WriteNoBreakIndent("\", \"");
            WriteFormated(
#line 43 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
                                                                                                                     $"{(typeof(TemplateFile).Assembly.GetName().Version)}");
#line hidden
            WriteNoBreakIndent("\")]");
#line 44 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
                }
#line hidden
            WriteLine();
            WriteNoBreakIndent("    ");
            WriteFormated(
#line 45 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
     $"{(Modifier != TemplateVisibility.none ? Modifier.ToString() + " " : "")}");
#line hidden
            WriteNoBreakIndent("partial class ");
            WriteFormated(
#line 45 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
                                                                                              $"{(ClassName)}");
#line hidden
            WriteNoBreakIndent("Base");
            WriteLine();
            WriteNoBreakIndent("    {");
            WriteLine();
            WriteNoBreakIndent("        public ");
            WriteFormated(
#line 47 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
                $"{(ClassName)}");
#line hidden
            WriteNoBreakIndent("Context Context");
            WriteLine();
            WriteNoBreakIndent("        {");
            WriteLine();
            WriteNoBreakIndent("            get");
            WriteLine();
            WriteNoBreakIndent("            {");
            WriteLine();
            WriteNoBreakIndent("                if (null == _priv_Context)");
            WriteLine();
            WriteNoBreakIndent("                {");
            WriteLine();
            WriteNoBreakIndent("                    _priv_Context = new ");
            WriteFormated(
#line 53 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
                                         $"{(ClassName)}");
#line hidden
            WriteNoBreakIndent("Context();");
            WriteLine();
            WriteNoBreakIndent("                }");
            WriteLine();
            WriteNoBreakIndent("                return _priv_Context;");
            WriteLine();
            WriteNoBreakIndent("            }");
            WriteLine();
            WriteLine();
            WriteNoBreakIndent("            set");
            WriteLine();
            WriteNoBreakIndent("            {");
            WriteLine();
            WriteNoBreakIndent("                _priv_Context = value;");
            WriteLine();
            WriteNoBreakIndent("            }");
            WriteLine();
            WriteNoBreakIndent("        }");
            WriteLine();
            WriteNoBreakIndent("        private ");
            WriteFormated(
#line 63 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
                 $"{(ClassName)}");
#line hidden
            WriteNoBreakIndent("Context _priv_Context;");
            WriteLine();
            WriteLine();
            WriteNoBreakIndent("#line default");
            WriteLine();
            WriteNoBreakIndent("        public static bool EndsWithNewLine(string text)");
            WriteLine();
            WriteNoBreakIndent("        {");
            WriteLine();
            WriteNoBreakIndent("            if (string.IsNullOrEmpty(text)) return false;");
            WriteLine();
            WriteNoBreakIndent("            return IsNewlineAtIndex(text, text.Length - 1);");
            WriteLine();
            WriteNoBreakIndent("        }");
            WriteLine();
            WriteLine();
            WriteNoBreakIndent("        public static bool IsNewlineAtIndex(global::System.ReadOnlySpan<char> text, int offset, out int len)");
            WriteLine();
            WriteNoBreakIndent("        {");
            WriteLine();
            WriteNoBreakIndent("            len = 0;");
            WriteLine();
            WriteNoBreakIndent("            if (offset >= text.Length) return false;");
            WriteLine();
            WriteNoBreakIndent("            var ch = text[offset];");
            WriteLine();
            WriteNoBreakIndent("            if (ch == \'\\r\' && offset + 1 < text.Length && text[offset + 1] == \'\\n\')");
            WriteLine();
            WriteNoBreakIndent("            {");
            WriteLine();
            WriteNoBreakIndent("                len = 2;");
            WriteLine();
            WriteNoBreakIndent("            }");
            WriteLine();
            WriteNoBreakIndent("            else if // as coded in unicode, except CR+LF case is above");
            WriteLine();
            WriteNoBreakIndent("                (ch == \'\\r\' // cr");
            WriteLine();
            WriteNoBreakIndent("                | ch == \'\\n\' // LF ");
            WriteLine();
            WriteNoBreakIndent("                | ch == \'\\x000b\' // VT");
            WriteLine();
            WriteNoBreakIndent("                | ch == \'\\x000c\' // FF");
            WriteLine();
            WriteNoBreakIndent("                | ch == \'\\x0085\' // NEL");
            WriteLine();
            WriteNoBreakIndent("                | ch == \'\\x2028\' // LS");
            WriteLine();
            WriteNoBreakIndent("                | ch == \'\\x2029\' // PS");
            WriteLine();
            WriteNoBreakIndent("                )");
            WriteLine();
            WriteNoBreakIndent("            {");
            WriteLine();
            WriteNoBreakIndent("                len = 1;");
            WriteLine();
            WriteNoBreakIndent("            }");
            WriteLine();
            WriteNoBreakIndent("            return len > 0;");
            WriteLine();
            WriteNoBreakIndent("        }");
            WriteLine();
            WriteLine();
            WriteNoBreakIndent("        public static bool IsNewlineAtIndex(global::System.ReadOnlySpan<char> text, int index)");
            WriteLine();
            WriteNoBreakIndent("        {");
            WriteLine();
            WriteNoBreakIndent("            if (index >= text.Length) return false;");
            WriteLine();
            WriteNoBreakIndent("            switch (text[index])");
            WriteLine();
            WriteNoBreakIndent("            {");
            WriteLine();
            WriteNoBreakIndent("                case \'\\r\': // cr");
            WriteLine();
            WriteNoBreakIndent("                case \'\\n\': // LF");
            WriteLine();
            WriteNoBreakIndent("                case \'\\x000b\': // VT");
            WriteLine();
            WriteNoBreakIndent("                case \'\\x000c\': // FF");
            WriteLine();
            WriteNoBreakIndent("                case \'\\x0085\': // NEL");
            WriteLine();
            WriteNoBreakIndent("                case \'\\x2028\': // LS");
            WriteLine();
            WriteNoBreakIndent("                case \'\\x2029\': // PS");
            WriteLine();
            WriteNoBreakIndent("                    return true;");
            WriteLine();
            WriteNoBreakIndent("                default:");
            WriteLine();
            WriteNoBreakIndent("                    return false;");
            WriteLine();
            WriteNoBreakIndent("            }");
            WriteLine();
            WriteNoBreakIndent("        }");
            WriteLine();
            WriteLine();
            WriteNoBreakIndent("        public static bool IsNewlineAtIndex(string text, int index)");
            WriteLine();
            WriteNoBreakIndent("        {");
            WriteLine();
            WriteNoBreakIndent("            if (string.IsNullOrEmpty(text) || index >= text.Length) return false;");
            WriteLine();
            WriteNoBreakIndent("            switch (text[index])");
            WriteLine();
            WriteNoBreakIndent("            {");
            WriteLine();
            WriteNoBreakIndent("                case \'\\r\': // cr");
            WriteLine();
            WriteNoBreakIndent("                case \'\\n\': // LF");
            WriteLine();
            WriteNoBreakIndent("                case \'\\x000b\': // VT");
            WriteLine();
            WriteNoBreakIndent("                case \'\\x000c\': // FF");
            WriteLine();
            WriteNoBreakIndent("                case \'\\x0085\': // NEL");
            WriteLine();
            WriteNoBreakIndent("                case \'\\x2028\': // LS");
            WriteLine();
            WriteNoBreakIndent("                case \'\\x2029\': // PS");
            WriteLine();
            WriteNoBreakIndent("                    return true;");
            WriteLine();
            WriteNoBreakIndent("                default:");
            WriteLine();
            WriteNoBreakIndent("                    return false;");
            WriteLine();
            WriteNoBreakIndent("            }");
            WriteLine();
            WriteNoBreakIndent("        }");
            WriteLine();
            WriteLine();
            WriteNoBreakIndent("        protected void Write(string text)");
            WriteLine();
            WriteNoBreakIndent("        {");
            WriteLine();
            WriteNoBreakIndent("            if (text == null || text.Length == 0) return;");
            WriteLine();
            WriteNoBreakIndent("            if (Context.Writer == null) throw new global::System.InvalidOperationException(\"Context.Writer is not initialized\");");
            WriteLine();
            WriteLine();
            WriteNoBreakIndent("            global::System.ReadOnlySpan<char> textSpan = global::System.MemoryExtensions.AsSpan(text);");
            WriteLine();
            WriteNoBreakIndent("            int index = 0;");
            WriteLine();
            WriteNoBreakIndent("            int textStartIndex = 0;");
            WriteLine();
            WriteNoBreakIndent("            while (index < textSpan.Length)");
            WriteLine();
            WriteNoBreakIndent("            {");
            WriteLine();
            WriteNoBreakIndent("                if (IsNewlineAtIndex(textSpan, index, out int len))");
            WriteLine();
            WriteNoBreakIndent("                {");
            WriteLine();
            WriteNoBreakIndent("                    if (index != textStartIndex) PutIndent();");
            WriteLine();
            WriteNoBreakIndent("                    WriteParts(textSpan, textStartIndex, index + len);");
            WriteLine();
            WriteNoBreakIndent("                    index += len;");
            WriteLine();
            WriteNoBreakIndent("                    Context.EoL = true;");
            WriteLine();
            WriteNoBreakIndent("                    textStartIndex = index;");
            WriteLine();
            WriteNoBreakIndent("                }");
            WriteLine();
            WriteNoBreakIndent("                else");
            WriteLine();
            WriteNoBreakIndent("                {");
            WriteLine();
            WriteNoBreakIndent("                    index++;");
            WriteLine();
            WriteNoBreakIndent("                }");
            WriteLine();
            WriteNoBreakIndent("            }");
            WriteLine();
            WriteNoBreakIndent("            if (index != textStartIndex)");
            WriteLine();
            WriteNoBreakIndent("            {");
            WriteLine();
            WriteNoBreakIndent("                PutIndent();");
            WriteLine();
            WriteNoBreakIndent("                WriteParts(textSpan, textStartIndex, index);");
            WriteLine();
            WriteNoBreakIndent("                Context.EoL = false;");
            WriteLine();
            WriteNoBreakIndent("            }");
            WriteLine();
            WriteNoBreakIndent("        }");
            WriteLine();
            WriteLine();
            WriteNoBreakIndent("        private void WriteParts(global::System.ReadOnlySpan<char> textSpan, int offset1, int offset2)");
            WriteLine();
            WriteNoBreakIndent("        {");
            WriteLine();
            WriteNoBreakIndent("            Context.Writer.Write(textSpan.Slice(offset1, offset2 - offset1));");
            WriteLine();
            WriteNoBreakIndent("        }");
            WriteLine();
            WriteLine();
            WriteNoBreakIndent("        private void PutIndent()");
            WriteLine();
            WriteNoBreakIndent("        {");
            WriteLine();
            WriteNoBreakIndent("            if (Context.EoL)");
            WriteLine();
            WriteNoBreakIndent("            {");
            WriteLine();
            WriteNoBreakIndent("                foreach (var indent in System.Linq.Enumerable.Reverse(Context.Indents)) Context.Writer.Write(indent);");
            WriteLine();
            WriteNoBreakIndent("            }");
            WriteLine();
            WriteNoBreakIndent("        }");
            WriteLine();
            WriteLine();
            WriteNoBreakIndent("        protected void WriteNoBreakIndent(string text)");
            WriteLine();
            WriteNoBreakIndent("        {");
            WriteLine();
            WriteNoBreakIndent("            if (Context.Writer == null) throw new System.InvalidOperationException(\"Context.Writer is not initialized\");");
            WriteLine();
            WriteNoBreakIndent("            if (text == null || text.Length == 0) return;");
            WriteLine();
            WriteNoBreakIndent("            PutIndent();");
            WriteLine();
            WriteNoBreakIndent("            Context.Writer.Write(text);");
            WriteLine();
            WriteNoBreakIndent("            Context.EoL = false;");
            WriteLine();
            WriteNoBreakIndent("        }");
            WriteLine();
            WriteLine();
            WriteNoBreakIndent("        protected void WriteFormated(global::System.FormattableString text)");
            WriteLine();
            WriteNoBreakIndent("        {");
            WriteLine();
            WriteNoBreakIndent("            Write(text.ToString(Context.FormatProvider ?? global::System.Globalization.CultureInfo.CurrentUICulture));");
            WriteLine();
            WriteNoBreakIndent("        }");
            WriteLine();
            WriteLine();
            WriteNoBreakIndent("        protected void WriteLine()");
            WriteLine();
            WriteNoBreakIndent("        {");
            WriteLine();
            WriteNoBreakIndent("            if (Context.Writer == null) throw new System.InvalidOperationException(\"Context.Writer is not initialized\");");
            WriteLine();
            WriteNoBreakIndent("            Context.Writer.WriteLine();");
            WriteLine();
            WriteNoBreakIndent("            Context.EoL = true;");
            WriteLine();
            WriteNoBreakIndent("        }");
            WriteLine();
            WriteLine();
            WriteNoBreakIndent("        protected void WriteLine(string text)");
            WriteLine();
            WriteNoBreakIndent("        {");
            WriteLine();
            WriteNoBreakIndent("            Write(text);");
            WriteLine();
            WriteNoBreakIndent("            WriteLine();");
            WriteLine();
            WriteNoBreakIndent("        }");
            WriteLine();
            WriteLine();
            WriteNoBreakIndent("        protected void PushIndent(string indent)");
            WriteLine();
            WriteNoBreakIndent("        {");
            WriteLine();
            WriteNoBreakIndent("            Context.Indents.Push(indent);");
            WriteLine();
            WriteNoBreakIndent("        }");
            WriteLine();
            WriteLine();
            WriteNoBreakIndent("        protected string PopIndent()");
            WriteLine();
            WriteNoBreakIndent("        {");
            WriteLine();
            WriteNoBreakIndent("            return Context.Indents.Pop();");
            WriteLine();
            WriteNoBreakIndent("        }");
            WriteLine();
            WriteLine();
            WriteNoBreakIndent("        protected void SkipIndent()");
            WriteLine();
            WriteNoBreakIndent("        {");
            WriteLine();
            WriteNoBreakIndent("            Context.EoL = false;");
            WriteLine();
            WriteNoBreakIndent("        }");
            WriteLine();
            WriteNoBreakIndent("#line default");
            WriteLine();
            WriteNoBreakIndent("    }");
            WriteLine();
            WriteLine();
            WriteNoBreakIndent("    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"");
            WriteFormated(
#line 220 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
                                                              $"{(typeof(TemplateFile).Assembly.GetName().Name)}");
#line hidden
            WriteNoBreakIndent("\", \"");
            WriteFormated(
#line 220 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
                                                                                                                     $"{(typeof(TemplateFile).Assembly.GetName().Version)}");
#line hidden
            WriteNoBreakIndent("\")]");
            WriteLine();
            WriteNoBreakIndent("    ");
            WriteFormated(
#line 221 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
     $"{(Modifier != TemplateVisibility.none ? Modifier.ToString() + " " : "")}");
#line hidden
            WriteNoBreakIndent("partial class ");
            WriteFormated(
#line 221 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
                                                                                              $"{(ClassName)}");
#line hidden
            WriteNoBreakIndent("Context");
            WriteLine();
            WriteNoBreakIndent("    {");
            WriteLine();
            WriteNoBreakIndent("        public global::System.IO.TextWriter Writer { get; set; }");
            WriteLine();
            WriteNoBreakIndent("        public bool EoL { get; set; } = true;");
            WriteLine();
            WriteNoBreakIndent("        public global::System.Collections.Generic.Stack<string> Indents { get; } = new global::System.Collections.Generic.Stack<string>();");
            WriteLine();
            WriteNoBreakIndent("        public global::System.IFormatProvider FormatProvider { get; set; }");
            WriteLine();
            WriteNoBreakIndent("    }");
            WriteLine();
#line 228 "D:\workspace\QuickTemplates\Michaelis.QuickTemplates\templates\TemplateFile.tt"
            }
#line hidden
            WriteLine();
            WriteNoBreakIndent("}");
        }
#line default
    }

}