using Listing.Code;
using Microsoft.CodeAnalysis;

namespace Listing.Contents;

public readonly struct Type : IContent
{
    private sealed class Visitor : SymbolVisitor
    {
        private readonly Output output;
        private readonly Type type;

        public Visitor(Output output, Type type)
        {
            this.output = output;
            this.type = type;
        }

        public override void VisitNamedType(INamedTypeSymbol symbol)
        {
            var @namespace = new Namespace(
                symbol.ContainingNamespace,
                verbatimPrefix: VerbatimPrefix,
                globalPrefix: GlobalPrefix
            );
            output.Write(@namespace);
            if (@namespace.DotSeparatorNeeded)
            {
                output.Write(".");
            }

            var list = output.Separated(".");
            Print(symbol);

            void Print(INamedTypeSymbol s)
            {
                if (s.ContainingType is { } outer)
                {
                    Print(outer);
                }

                if (VerbatimPrefix)
                {
                    list.Write("@".AsContent());
                }

                list.Write(s.Name.AsContent());
                if (s.TypeParameters.Length > 0)
                {
                    list.Write("<".AsContent());

                    for (var i = 0; i < s.TypeParameters.Length; i++)
                    {
                        var parameter = s.TypeArguments[i];
                        Visit(parameter);
                        if (i != s.TypeParameters.Length - 1)
                        {
                            list.Write(", ".AsContent());
                        }
                    }

                    list.Write(">".AsContent());
                }

                list.EndItem();
            }
        }

        public override void VisitTypeParameter(ITypeParameterSymbol symbol)
        {
            if (VerbatimPrefix)
            {
                output.Write("@");
            }

            output.Write(symbol.Name);
        }

        private bool GlobalPrefix => type.globalPrefix;

        private bool VerbatimPrefix => type.verbatimPrefix;
    }

    private static readonly string?[] arraySuffixesCache = new string?[16];
    private readonly ITypeSymbol symbol;
    private readonly bool globalPrefix;
    private readonly bool verbatimPrefix;

    public Type(ITypeSymbol symbol, bool verbatimPrefix = true, bool globalPrefix = true)
    {
        this.symbol = symbol;
        this.verbatimPrefix = verbatimPrefix;
        this.globalPrefix = globalPrefix;
    }

    public void Write(Output output)
    {
        switch (symbol.SpecialType)
        {
            case SpecialType.System_Boolean:
                output.Write("bool");
                return;
            case SpecialType.System_Byte:
                output.Write("byte");
                return;
            case SpecialType.System_SByte:
                output.Write("sbyte");
                return;
            case SpecialType.System_Int16:
                output.Write("short");
                return;
            case SpecialType.System_UInt16:
                output.Write("ushort");
                return;
            case SpecialType.System_Int32:
                output.Write("int");
                return;
            case SpecialType.System_UInt32:
                output.Write("uint");
                return;
            case SpecialType.System_Int64:
                output.Write("long");
                return;
            case SpecialType.System_UInt64:
                output.Write("ulong");
                return;
            case SpecialType.System_Object:
                output.Write("object");
                return;
            case SpecialType.System_Decimal:
                output.Write("decimal");
                return;
            case SpecialType.System_Single:
                output.Write("float");
                return;
            case SpecialType.System_Double:
                output.Write("double");
                return;
            case SpecialType.System_String:
                output.Write("string");
                return;
            case SpecialType.System_Char:
                output.Write("char");
                return;
            case SpecialType.System_Void:
                output.Write("void");
                return;
        }

        var visitor = new Visitor(output, this);
        visitor.Visit(symbol);
        return;

        var @this = this;
        Print(symbol);

        void Print(ITypeSymbol type)
        {
            if (type is IErrorTypeSymbol error)
            {
                output.Write(error.ToDisplayString());
                return;
            }

            if (type is IArrayTypeSymbol arrayType)
            {
                var arrays = new Queue<IArrayTypeSymbol>();
                ITypeSymbol element = arrayType;
                for (; element is IArrayTypeSymbol arrayElement; element = arrayElement.ElementType)
                {
                    arrays.Enqueue(arrayElement);
                }

                Print(element);
                foreach (var arrayTypeSymbol in arrays)
                {
                    output.Write(GetArraySuffix(arrayTypeSymbol.Rank));
                }

                return;
            }

            if (type is INamedTypeSymbol namedType)
            {
                if (namedType is { IsGenericType: true, TypeArguments.Length: > 0 } && namedType.TypeArguments.All(argument => argument.Kind != SymbolKind.TypeParameter))
                {
                    Print(namedType.ConstructedFrom);
                    output.Write("<");
                    var list = output.CommaSeparated();

                    foreach (var argument in namedType.TypeArguments)
                    {
                        list.WriteItem(new Type(argument, verbatimPrefix: @this.verbatimPrefix, globalPrefix: @this.globalPrefix));
                    }

                    output.Write(">");
                    return;
                }
            }

            var outer = type.ContainingType;
            if (outer == null)
            {
                var @namespace = new Namespace(
                    type.ContainingNamespace,
                    @this.globalPrefix,
                    verbatimPrefix: @this.verbatimPrefix
                );
                output.Write(@namespace);

                if (@namespace.DotSeparatorNeeded)
                {
                    output.Write(".");
                }

                if (@this.verbatimPrefix)
                {
                    output.Write("@");
                }

                output.Write(type.Name);
                return;
            }

            Print(outer);
            output.Write(@this.verbatimPrefix ? ".@" : ".");
            output.Write(type.Name);
        }
    }

    private static string GetArraySuffix(int rank)
    {
        if (rank < arraySuffixesCache.Length)
        {
            if (arraySuffixesCache[rank] is { } value)
            {
                return value;
            }

            var suffix = Suffix();
            arraySuffixesCache[rank] = suffix;
            return suffix;
        }

        return Suffix();

        string Suffix() => $"[{new string(',', rank - 1)}]";
    }

    public override string ToString() => this.Print();
}