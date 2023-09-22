using System.Diagnostics.Contracts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Listing.Tests;

public sealed class SimpleCompilation
{
    public string Source { get; }
    public CSharpCompilation Content { get; }

    public INamedTypeSymbol Locate(string path)
    {
        INamespaceOrTypeSymbol symbol = Content.GlobalNamespace;
        foreach (var waypoint in path.Split('.'))
        {
            symbol = symbol.GetMembers(waypoint).OfType<INamespaceOrTypeSymbol>().Single();
        }

        return (INamedTypeSymbol)symbol;
    }

    private SimpleCompilation(string source, CSharpCompilation content)
    {
        Source = source;
        Content = content;
    }

    [Pure]
    public static SimpleCompilation Create(string source)
    {
        var references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        var syntaxTrees = new[]
        {
            CSharpSyntaxTree.ParseText(source)
        };
        var compilation = CSharpCompilation.Create(
            "Tests",
            syntaxTrees,
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        return new SimpleCompilation(source, compilation);
    }
}