using System.Globalization;
using Listing.Contents;

namespace Listing.Code;

public readonly struct Enum : IDisposable
{
    private readonly Output output;
    private readonly Output.Block block;

    private Enum(Output output, Output.Block block)
    {
        this.block = block;
        this.output = output;
    }

    public static Enum Open<T>(Output output, AccessModifier modifiers, T name, UnderlyingType underlyingType = UnderlyingType.Int) where T : IContent
    {
        using (output.StartLine())
        {
            var list = output.Separated(" ");
            list.TryWriteItem(modifiers.AsContent());
            list.WriteItem("enum".AsContent());
            list.WriteItem(name);
            if (underlyingType != UnderlyingType.Int)
            {
                list.WriteItem(":".AsContent());
                var content = underlyingType switch
                {
                    UnderlyingType.Byte => "byte",
                    UnderlyingType.Sbyte => "sbyte",
                    UnderlyingType.Ushort => "ushort",
                    UnderlyingType.Short => "short",
                    UnderlyingType.Uint => "uint",
                    UnderlyingType.Ulong => "ulong",
                    UnderlyingType.Long => "long",
                    _ => "int"
                };
                list.WriteItem(content.AsContent());
            }
        }

        var block = output.OpenBlock();
        return new Enum(output, block);
    }

    public void Member<T>(T name, int value) where T : IContent
    {
        using (output.StartLine())
        {
            output.Write(name);
            output.Write(" = ");
            output.Write(value.ToString(CultureInfo.InvariantCulture));
            output.Write(",");
        }
    }

    public void Member<T>(T name) where T : IContent
    {
        using (output.StartLine())
        {
            output.Write(name);
            output.Write(",");
        }
    }

    public void Dispose()
    {
        block.Dispose();
    }

    public enum UnderlyingType : byte
    {
        Byte,
        Sbyte,
        Ushort,
        Short,
        Uint,
        Int,
        Ulong,
        Long
    }
}