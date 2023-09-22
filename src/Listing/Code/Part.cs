using Listing.Contents;
using Microsoft.CodeAnalysis;

namespace Listing.Code;

public sealed class Part
{
    private readonly IContent? additionalModifiers;
    private readonly INamedTypeSymbol symbol;
    private readonly IContent? baseType;
    // todo generic type constraint

    public Part(IContent? additionalModifiers, INamedTypeSymbol symbol, IContent? baseType)
    {
        this.additionalModifiers = additionalModifiers;
        this.symbol = symbol;
        this.baseType = baseType;
    }

    public Part(INamedTypeSymbol symbol)
        : this(null, symbol, null)
    {
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

        OpenType(symbol, root: true);
        return new MultiBlock(output, blocks);

        void OpenType(INamedTypeSymbol type, bool root = false)
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

                WriteDeclaration(root, declaration);
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

                WriteDeclaration(root, declaration);
            }
            else
            {
                // todo enrich warning
                WriteDeclaration(root, type.Name.AsContent());
            }

            _ = output.OpenBlock();
            blocks++;
        }

        void WriteDeclaration<T>(bool root, in T declaration) where T : IContent
        {
            using (output.StartLine())
            {
                var list = output.Separated(" ");
                if (root)
                {
                    list.TryWriteItem(additionalModifiers);
                }

                list.WriteItem(declaration);

                if (root && baseType is { } @base)
                {
                    list.WriteItem(":".AsContent());
                    list.WriteItem(@base);
                }
            }
        }
    }
}