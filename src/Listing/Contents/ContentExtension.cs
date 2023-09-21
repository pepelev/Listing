using System.Text;
using Listing.Code;

namespace Listing.Contents;

public static class ContentExtension
{
    public static string Print(this IContent content)
    {
        var output = new Output(new StringBuilder(128));
        content.Write(output);
        return output.ToString();
    }

    public static VerbatimPrefixed<T> VerbatimPrefixed<T>(this T content) where T : IContent => new(content);
    public static Verbatim AsContent(this string? content) => new(content ?? "");
}