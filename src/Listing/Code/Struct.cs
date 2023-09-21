using Listing.Contents;

namespace Listing.Code;

internal static class Struct
{
    public static Type<T> Open<T>(Output output, string modifiers, T name) where T : IContent
        => Type<T>.Open(output, modifiers, "struct", name);
}