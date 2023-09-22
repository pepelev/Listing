using Listing.Code;
using Microsoft.CodeAnalysis;

namespace Listing.Contents;

public readonly struct Namespace : IContent
{
    private readonly INamespaceSymbol symbol;
    private readonly bool globalPrefix;
    private readonly bool verbatimPrefix;

    public Namespace(INamespaceSymbol symbol, bool globalPrefix, bool verbatimPrefix = true)
    {
        this.symbol = symbol;
        this.globalPrefix = globalPrefix;
        this.verbatimPrefix = verbatimPrefix;
    }

    public bool IsGlobal => symbol.IsGlobalNamespace;
    public bool DotSeparatorNeeded => !IsGlobal;

    public void Write(Output output)
    {
        if (IsGlobal)
        {
            if (globalPrefix)
            {
                output.Write("global::");
                return;
            }

            return;
        }

        var @this = this;
        Print(symbol);

        void Print(INamespaceSymbol @namespace)
        {
            var outer = @namespace.ContainingNamespace;
            if (outer == null || outer.IsGlobalNamespace)
            {
                if (@this.globalPrefix)
                {
                    output.Write("global::");
                }

                if (@this.verbatimPrefix)
                {
                    output.Write("@");
                }

                output.Write(@namespace.Name);
            }
            else
            {
                Print(outer);
                output.Write(@this.verbatimPrefix ? ".@" : ".");
                output.Write(@namespace.Name);
            }
        }
    }

    public override string ToString() => this.Print();
}