using Listing.Contents;

namespace Listing.Code;

internal static class Class
{
    public static Type<T> Open<T>(Output output, string modifiers, T name) where T : IContent
        => Type<T>.Open(output, modifiers, "class", name);
}