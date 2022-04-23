using System;
using System.Text;

namespace Michaelis.QuickTemplates.Text;

public static class StringUtils
{
    [ThreadStatic]
    private static StringBuilder StringBuilderInstance;
    private const int MAX_STRING_BUILDER_SIZE = 1024;

    public static StringBuilder AcquireStringBuilder(int capacity = MAX_STRING_BUILDER_SIZE)
    {
        if (capacity <= MAX_STRING_BUILDER_SIZE)
        {
            StringBuilder sb = StringBuilderInstance;
            if (sb != null)
            {
                return sb;
            }
        }
        return new StringBuilder(capacity);
    }

    public static string GetStringAndRelease(StringBuilder sb)
    {
        string result = sb.ToString();
        if (sb.Capacity <= MAX_STRING_BUILDER_SIZE)
        {
            StringBuilderInstance = sb;
            sb.Clear();
        }
        return result;
    }

    public static string SafeEncodeString(string data)
    {
        var encodeBuilder = AcquireStringBuilder(data.Length + 2);

        encodeBuilder.Append('"');
        for (int i = 0; i < data.Length; i++)
        {
            var ch = data[i];
            switch (ch)
            {
                case '\'': encodeBuilder.Append(@"\'"); break;
                case '\"': encodeBuilder.Append("\\\""); break;
                case '\\': encodeBuilder.Append(@"\\"); break;
                case '\0': encodeBuilder.Append(@"\0"); break;
                case '\a': encodeBuilder.Append(@"\a"); break;
                case '\b': encodeBuilder.Append(@"\b"); break;
                case '\f': encodeBuilder.Append(@"\f"); break;
                case '\n': encodeBuilder.Append(@"\n"); break;
                case '\r': encodeBuilder.Append(@"\r"); break;
                case '\t': encodeBuilder.Append(@"\t"); break;
                case '\v': encodeBuilder.Append(@"\v"); break;
                default:
                    if (ch >= 0x20 && ch <= 0x7e) //ASCII printable
                    {
                        encodeBuilder.Append(ch);
                    }
                    else if (char.IsLetterOrDigit(ch) || (char.IsPunctuation(ch)))
                    {
                        encodeBuilder.Append(ch);
                    }
                    else
                    {
                        encodeBuilder.Append($"\\u{ch:x04}");
                    }
                    break;
            }
        }
        encodeBuilder.Append('"');
        return GetStringAndRelease(encodeBuilder);
    }
}
