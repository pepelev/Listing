using Listing.Contents;
using Microsoft.CodeAnalysis;

namespace Listing.Code;

public readonly struct Part
{
    private readonly INamedTypeSymbol symbol;

    public Part(INamedTypeSymbol symbol)
    {
        this.symbol = symbol;
    }

    public MultiBlock Open(Output output)
    {
        var blocks = 0;
        var @namespace = symbol.ContainingNamespace;
        if (!@namespace.IsGlobalNamespace)
        {
            _ = output.OpenNamespace(new Namespace(@namespace, globalPrefix: false));
            blocks++;
        }

        OpenType(symbol);
        return new MultiBlock(output, blocks);

        void OpenType(INamedTypeSymbol type)
        {
            if (type.ContainingType is { } outer)
            {
                OpenType(outer);
            }

            if (type.IsReferenceType)
            {
                var modifiers = ReferenceTypeModifiers.Default.Partial();
                if (type.IsRecord)
                {
                    modifiers = modifiers.Record();
                }

                var declaration = new TypeDeclaration<ReferenceTypeModifiers>(
                    modifiers,
                    type.Name,
                    type.TypeParameters
                );
                output.WriteLine(declaration);
            }
            else if (type.IsValueType)
            {
                var modifiers = ValueTypeModifiers.Default.Partial();
                if (type.IsRecord)
                {
                    modifiers = modifiers.Record();
                }
                    
                var declaration = new TypeDeclaration<ValueTypeModifiers>(
                    modifiers,
                    type.Name,
                    type.TypeParameters
                );
                output.WriteLine(declaration);
                    
            }
            else
            {
                // todo enrich warning
                output.WriteLine(type.Name.AsContent());
            }

            _ = output.OpenBlock();
            blocks++;
        }
    }
}