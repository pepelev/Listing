using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using Listing.Contents;
using Microsoft.CodeAnalysis;

namespace Listing.Code;

public readonly struct TypeDeclaration<TModifiers> : IContent
    where TModifiers : IContent
{
    private readonly TModifiers modifiers;
    private readonly ImmutableArray<ITypeParameterSymbol> typeParameters;

    public TModifiers Modifiers => modifiers;

    public string Name { get; }

    public TypeDeclaration(
        TModifiers modifiers,
        string name,
        ImmutableArray<ITypeParameterSymbol> typeParameters)
    {
        this.modifiers = modifiers;
        Name = name;
        this.typeParameters = typeParameters;
    }

    public void Write(Output output)
    {
        output.Write(in modifiers);
        output.Write(" ");
        output.Write(Name.AsContent().VerbatimPrefixed());

        if (typeParameters.Length > 0)
        {
            output.Write("<");
            var list = output.CommaSeparated();
            foreach (var type in typeParameters)
            {
                list.WriteItem(type.Name.AsContent().VerbatimPrefixed());
            }
            output.Write(">");
        }

        // todo record primary ctor
    }

    public override string ToString() => this.Print();
}

// todo static, non static
public readonly struct ReferenceTypeModifiers : IContent
{
    private readonly Kind kind;
    private readonly Inheritance inheritance;

    public ReferenceTypeModifiers(
        AccessModifier access,
        Inheritance inheritance,
        PartialModifier partialModifier,
        Kind kind)
    {
        this.kind = kind;
        this.inheritance = inheritance;
        Access = access;
        PartialModifier = partialModifier;
    }

    [Pure]
    public static ReferenceTypeModifiers From(INamedTypeSymbol symbol)
    {
        if (!symbol.IsReferenceType)
        {
            throw new ArgumentException("Reference type expected", nameof(symbol));
        }

        var access = symbol.DeclaredAccessibility switch
        {
            Accessibility.Public => AccessModifier.Public,
            Accessibility.Internal => AccessModifier.Internal,
            Accessibility.Private => AccessModifier.Private,
            _ => throw new NotImplementedException()
        };
        var inheritance = symbol switch
        {
            { IsAbstract: true } => Inheritance.Abstract,
            { IsSealed: true } => Inheritance.Sealed,
            _ => Inheritance.Unspecified
        };
        // todo
        // var partial =
        var kind = symbol.IsRecord
            ? Kind.Record
            : Kind.Class;
        return new ReferenceTypeModifiers(access, inheritance, PartialModifier.NotPartial, kind);
    }

    public static ReferenceTypeModifiers Default => new(
        AccessModifier.Default,
        Inheritance.Unspecified,
        PartialModifier.NotPartial,
        Kind.Class
    );

    public static ReferenceTypeModifiers DefaultPartial => new(
        AccessModifier.Default,
        Inheritance.Unspecified,
        PartialModifier.Partial,
        Kind.Class
    );

    [Pure]
    public ReferenceTypeModifiers Public() => new(AccessModifier.Public, inheritance, PartialModifier, kind);

    [Pure]
    public ReferenceTypeModifiers Internal() => new(AccessModifier.Internal, inheritance, PartialModifier, kind);

    [Pure]
    public ReferenceTypeModifiers Private() => new(AccessModifier.Private, inheritance, PartialModifier, kind);

    [Pure]
    public ReferenceTypeModifiers UnspecifiedInheritance() => new(Access, Inheritance.Unspecified, PartialModifier, kind);

    [Pure]
    public ReferenceTypeModifiers Sealed() => new(Access, Inheritance.Sealed, PartialModifier, kind);

    [Pure]
    public ReferenceTypeModifiers Abstract() => new(Access, Inheritance.Abstract, PartialModifier, kind);

    [Pure]
    public ReferenceTypeModifiers Partial() => new(Access, inheritance, PartialModifier.Partial, kind);

    [Pure]
    public ReferenceTypeModifiers Class() => new(Access, inheritance, PartialModifier, Kind.Class);

    [Pure]
    public ReferenceTypeModifiers Record() => new(Access, inheritance, PartialModifier, Kind.Record);

    public enum Kind : byte
    {
        Class,
        Record
    }

    public enum Inheritance : byte
    {
        Unspecified,
        Abstract,
        Sealed
    }

    public AccessModifier Access { get; }
    public PartialModifier PartialModifier { get; }

    public void Write(Output output)
    {
        var list = output.Separated(" ");
        list.TryWriteItem(Access.AsContent());
        list.TryWriteItem(inheritance.AsContent());
        list.TryWriteItem(PartialModifier.AsContent());
        list.WriteItem(kind.AsContent());
    }

    public override string ToString() => this.Print();
}