#nullable disable

namespace Michaelis.QuickTemplates
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Michaelis.QuickTemplates", "0.0.0.0")]
    internal partial class BaseTemplate : BaseTemplateBase
    {
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("qtg", "0.0.0.0")]
    internal partial class BaseTemplateBase
    {
        public BaseTemplateContext Context
        {
            get
            {
                if (null == _priv_Context)
                {
                    _priv_Context = new BaseTemplateContext();
                }
                return _priv_Context;
            }

            set
            {
                _priv_Context = value;
            }
        }
        private BaseTemplateContext _priv_Context;

#line default
        public static bool EndsWithNewLine(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            return IsNewlineAtIndex(text, text.Length - 1);
        }

        public static bool IsNewlineAtIndex(global::System.ReadOnlySpan<char> text, int offset, out int len)
        {
            len = 0;
            if (offset >= text.Length) return false;
            var ch = text[offset];
            if (ch == '\r' && offset + 1 < text.Length && text[offset + 1] == '\n')
            {
                len = 2;
            }
            else if // as coded in unicode, except CR+LF case is above
                (ch == '\r' // cr
                | ch == '\n' // LF 
                | ch == '\x000b' // VT
                | ch == '\x000c' // FF
                | ch == '\x0085' // NEL
                | ch == '\x2028' // LS
                | ch == '\x2029' // PS
                )
            {
                len = 1;
            }
            return len > 0;
        }

        public static bool IsNewlineAtIndex(global::System.ReadOnlySpan<char> text, int index)
        {
            if (index >= text.Length) return false;
            switch (text[index])
            {
                case '\r': // cr
                case '\n': // LF
                case '\x000b': // VT
                case '\x000c': // FF
                case '\x0085': // NEL
                case '\x2028': // LS
                case '\x2029': // PS
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsNewlineAtIndex(string text, int index)
        {
            if (string.IsNullOrEmpty(text) || index >= text.Length) return false;
            switch (text[index])
            {
                case '\r': // cr
                case '\n': // LF
                case '\x000b': // VT
                case '\x000c': // FF
                case '\x0085': // NEL
                case '\x2028': // LS
                case '\x2029': // PS
                    return true;
                default:
                    return false;
            }
        }

        protected void Write(string text)
        {
            if (text == null || text.Length == 0) return;
            if (Context.Writer == null) throw new global::System.InvalidOperationException("Context.Writer is not initialized");

            global::System.ReadOnlySpan<char> textSpan = global::System.MemoryExtensions.AsSpan(text);
            int index = 0;
            int textStartIndex = 0;
            while (index < textSpan.Length)
            {
                if (IsNewlineAtIndex(textSpan, index, out int len))
                {
                    if (index != textStartIndex) PutIndent();
                    WriteParts(textSpan, textStartIndex, index + len);
                    index += len;
                    Context.EoL = true;
                    textStartIndex = index;
                }
                else
                {
                    index++;
                }
            }
            if (index != textStartIndex)
            {
                PutIndent();
                WriteParts(textSpan, textStartIndex, index);
                Context.EoL = false;
            }
        }

        private void WriteParts(global::System.ReadOnlySpan<char> textSpan, int offset1, int offset2)
        {
            Context.Writer.Write(textSpan.Slice(offset1, offset2 - offset1));
        }

        private void PutIndent()
        {
            if (Context.EoL)
            {
                foreach (var indent in System.Linq.Enumerable.Reverse(Context.Indents)) Context.Writer.Write(indent);
            }
        }

        protected void WriteNoBreakIndent(string text)
        {
            if (Context.Writer == null) throw new System.InvalidOperationException("Context.Writer is not initialized");
            if (text == null || text.Length == 0) return;
            PutIndent();
            Context.Writer.Write(text);
            Context.EoL = false;
        }

        protected void WriteFormated(global::System.FormattableString text)
        {
            Write(text.ToString(Context.FormatProvider ?? global::System.Globalization.CultureInfo.CurrentUICulture));
        }

        protected void WriteLine()
        {
            if (Context.Writer == null) throw new System.InvalidOperationException("Context.Writer is not initialized");
            Context.Writer.WriteLine();
            Context.EoL = true;
        }

        protected void WriteLine(string text)
        {
            Write(text);
            WriteLine();
        }

        protected void PushIndent(string indent)
        {
            Context.Indents.Push(indent);
        }

        protected string PopIndent()
        {
            return Context.Indents.Pop();
        }

        protected void SkipIndent()
        {
            Context.EoL = false;
        }
#line default
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("qtg", "0.0.0.0")]
    internal partial class BaseTemplateContext
    {
        public global::System.IO.TextWriter Writer { get; set; }
        public bool EoL { get; set; } = true;
        public global::System.Collections.Generic.Stack<string> Indents { get; } = new global::System.Collections.Generic.Stack<string>();
        public global::System.IFormatProvider FormatProvider { get; set; }
    }

}
