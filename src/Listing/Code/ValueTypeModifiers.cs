using System.Diagnostics.Contracts;
using Listing.Contents;

namespace Listing.Code;

public readonly struct ValueTypeModifiers : IContent
{
    private readonly Kind kind;
    private readonly ReadonlyModifier @readonly;

    public ValueTypeModifiers(
        AccessModifier access,
        ReadonlyModifier @readonly,
        PartialModifier partialModifier,
        Kind kind)
    {
        Access = access;
        this.@readonly = @readonly;
        PartialModifier = partialModifier;
        this.kind = kind;
    }

    public static ValueTypeModifiers Default => new(
        AccessModifier.Default,
        ReadonlyModifier.NotReadonly,
        PartialModifier.NotPartial,
        Kind.Struct
    );

    [Pure]
    public ValueTypeModifiers Public() => new(AccessModifier.Public, @readonly, PartialModifier, kind);

    [Pure]
    public ValueTypeModifiers Internal() => new(AccessModifier.Internal, @readonly, PartialModifier, kind);

    [Pure]
    public ValueTypeModifiers Private() => new(AccessModifier.Private, @readonly, PartialModifier, kind);

    [Pure]
    public ValueTypeModifiers Readonly() => new(Access, ReadonlyModifier.Readonly, PartialModifier, kind);

    [Pure]
    public ValueTypeModifiers NotReadonly() => new(Access, ReadonlyModifier.NotReadonly, PartialModifier, kind);

    [Pure]
    public ValueTypeModifiers Partial() => new(Access, @readonly, PartialModifier.Partial, kind);

    [Pure]
    public ValueTypeModifiers Struct() => new(Access, @readonly, PartialModifier, Kind.Struct);

    [Pure]
    public ValueTypeModifiers Record() => new(Access, @readonly, PartialModifier, Kind.Record);

    public enum Kind : byte
    {
        Struct,
        Record
    }

    public enum ReadonlyModifier : byte
    {
        NotReadonly,
        Readonly
    }

    public AccessModifier Access { get; }
    public PartialModifier PartialModifier { get; }

    public void Write(Output output)
    {
        var list = output.Separated(" ");
        list.TryWriteItem(Access.AsContent());
        list.TryWriteItem(@readonly.AsContent());
        list.TryWriteItem(PartialModifier.AsContent());
        list.TryWriteItem(kind.AsContent());
    }

    public override string ToString() => this.Print();
}