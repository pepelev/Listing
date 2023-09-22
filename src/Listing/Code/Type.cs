using Listing.Contents;
using Microsoft.CodeAnalysis;
using Type = Listing.Contents.Type;

namespace Listing.Code
{
    public readonly struct Type<TName> : IDisposable
        where TName : IContent
    {
        private readonly TName name;
        private readonly Output output;
        private readonly Output.Block block;

        private Type(TName name, Output output, Output.Block block)
        {
            this.name = name;
            this.block = block;
            this.output = output;
        }

        public static Type<TName> Open(Output output, string modifiers, string type, in TName name)
        {
            using (output.StartLine())
            {
                output.Write(modifiers);
                output.Write(" ");
                output.Write(type);
                output.Write(" ");
                output.Write(name);
            }

            var block = output.OpenBlock();
            return new Type<TName>(name, output, block);
        }

        public static Type<Verbatim> OpenPart(Output output, INamedTypeSymbol symbol)
        {
            using (output.StartLine())
            {
                output.Write("partial");
                output.Write(" ");

                var kind = symbol.TypeKind switch
                {
                    TypeKind.Class when symbol.IsRecord => "record",
                    TypeKind.Class => "class",
                    TypeKind.Struct when symbol.IsRecord => "record struct",
                    TypeKind.Struct => "struct",
                    var typeKind => $"WRONG_{typeKind}"
                };

                output.Write(kind);
                output.Write(" ");
                output.Write(symbol.Name);

                var parameters = symbol.TypeParameters;
                if (parameters.Length > 0)
                {
                    output.Write("<");

                    var list = output.CommaSeparated();
                    foreach (var parameter in parameters)
                    {
                        list.WriteItem(new Verbatim(parameter.Name));
                    }

                    output.Write(">");
                }
            }

            var block = output.OpenBlock();
            return new Type<Verbatim>(new Verbatim(symbol.Name), output, block);
        }

        // todo rename
        public static Type<Verbatim> OpenPart2(Output output, INamedTypeSymbol symbol, INamedTypeSymbol @base)
        {
            using (output.StartLine())
            {
                output.Write("partial");
                output.Write(" ");

                var kind = symbol.TypeKind switch
                {
                    TypeKind.Class when symbol.IsRecord => "record",
                    TypeKind.Class => "class",
                    TypeKind.Struct when symbol.IsRecord => "record struct",
                    TypeKind.Struct => "struct",
                    var typeKind => $"WRONG_{typeKind}"
                };

                output.Write(kind);
                output.Write(" ");
                output.Write(symbol.Name);

                var parameters = symbol.TypeParameters;
                if (parameters.Length > 0)
                {
                    output.Write("<");

                    var list = output.CommaSeparated();
                    foreach (var parameter in parameters)
                    {
                        list.WriteItem(new Verbatim(parameter.Name));
                    }

                    output.Write(">");
                }

                output.Write(" : ");
                new Type(@base).Write(output);
            }

            var block = output.OpenBlock();
            return new Type<Verbatim>(new Verbatim(symbol.Name), output, block);
        }

        public void Dispose()
        {
            block.Dispose();
        }
    }
}