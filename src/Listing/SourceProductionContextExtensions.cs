using System.Text;
using Listing.Code;
using Listing.Contents;
using Microsoft.CodeAnalysis;

namespace Listing;

public static class SourceProductionContextExtensions
{
    private const int InitialCapacity = 80 * 1024 / sizeof(char);
    private static Output? cached;

    public static void AddSource(this SourceProductionContext context, string hintName, IContent content)
    {
        var read = Interlocked.Exchange(ref cached, null);
        var output = read ?? new Output(new StringBuilder(InitialCapacity));
        try
        {
            content.Write(output);
            context.AddSource(hintName, output.ToString());
        }
        finally
        {
            output.Clear();
            Interlocked.CompareExchange(ref cached, output, null);
        }
    }
}