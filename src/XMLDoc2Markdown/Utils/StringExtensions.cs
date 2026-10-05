using System;

namespace XMLDoc2Markdown.Utils;

internal static class StringExtensions
{
    internal static string FormatChevrons(this string value)
    {
        return value.Replace("<", "\\<").Replace(">", "\\>");
    }

    /// <summary>
    /// Escapes the characters of an XML doc text node that Markdown or MDX would otherwise read as markup:
    /// a backtick opens an inline code span, braces open an MDX expression. Chevrons need no handling here
    /// because <see cref="System.Xml.Linq.XText.ToString()"/> keeps them as <c>&amp;lt;</c>/<c>&amp;gt;</c> entities.
    /// </summary>
    internal static string EscapeMarkdownText(this string value)
    {
        return value
            .Replace("`", "\\`")
            .Replace("{", "\\{")
            .Replace("}", "\\}");
    }

    /// <summary>
    /// Wraps <paramref name="value"/> in a CommonMark code span. The backtick fence is one longer than the
    /// longest backtick run inside the value, and the content is padded with a space when it starts or ends
    /// with a backtick, so content such as <c>List`1</c> or <c>`quoted`</c> survives intact.
    /// </summary>
    internal static string ToInlineCode(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        int longestRun = 0;
        int run = 0;
        foreach (char @char in value)
        {
            run = @char == '`' ? run + 1 : 0;
            longestRun = Math.Max(longestRun, run);
        }

        string fence = new('`', longestRun + 1);
        string padding = value[0] == '`' || value[^1] == '`' ? " " : string.Empty;
        return $"{fence}{padding}{value}{padding}{fence}";
    }

    internal static string ToAnchorLink(this string value)
    {
        return value
            .Replace("(", string.Empty)
            .Replace(")", string.Empty)
            .Replace("<", string.Empty)
            .Replace(">", string.Empty)
            .Replace("[", string.Empty)
            .Replace("]", string.Empty)
            .Replace(",", string.Empty)
            .Replace(' ', '-')
            .ToLower();
    }
    

}
