using Listing.Contents;

namespace Listing.Code;

public static class EnumContentExtensions
{
    public static Verbatim? AsContent(this AccessModifier modifier) => modifier switch
    {
        AccessModifier.Public => "public",
        AccessModifier.Internal => "internal",
        AccessModifier.Private => "private",
        AccessModifier.Default => default(Verbatim?),
        _ => throw new ArgumentOutOfRangeException(nameof(modifier), modifier, null)
    };

    public static Verbatim? AsContent(this PartialModifier modifier) => modifier switch
    {
        PartialModifier.NotPartial => default(Verbatim?),
        PartialModifier.Partial => "partial",
        _ => throw new ArgumentOutOfRangeException(nameof(modifier), modifier, null)
    };

    public static Verbatim AsContent(this ReferenceTypeModifiers.Kind modifier) => modifier switch
    {
        ReferenceTypeModifiers.Kind.Class => "class",
        ReferenceTypeModifiers.Kind.Record => "record",
        _ => throw new ArgumentOutOfRangeException(nameof(modifier), modifier, null)
    };

    public static Verbatim? AsContent(this ValueTypeModifiers.Kind modifier) => modifier switch
    {
        ValueTypeModifiers.Kind.Struct => "struct",
        ValueTypeModifiers.Kind.Record => "record struct",
        _ => throw new ArgumentOutOfRangeException(nameof(modifier), modifier, null)
    };

    public static Verbatim? AsContent(this ReferenceTypeModifiers.Inheritance modifier) => modifier switch
    {
        ReferenceTypeModifiers.Inheritance.Unspecified => default(Verbatim?),
        ReferenceTypeModifiers.Inheritance.Abstract => "abstract",
        ReferenceTypeModifiers.Inheritance.Sealed => "sealed",
        _ => throw new ArgumentOutOfRangeException(nameof(modifier), modifier, null)
    };

    public static Verbatim? AsContent(this ValueTypeModifiers.ReadonlyModifier modifier) => modifier switch
    {
        ValueTypeModifiers.ReadonlyModifier.NotReadonly => default(Verbatim?),
        ValueTypeModifiers.ReadonlyModifier.Readonly => "readonly",
        _ => throw new ArgumentOutOfRangeException(nameof(modifier), modifier, null)
    };

    
}