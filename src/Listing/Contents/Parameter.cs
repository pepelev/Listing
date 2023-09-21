using Listing.Code;

namespace Listing.Contents;

public readonly struct Parameter<TType, TName> : IContent
    where TType : IContent
    where TName : IContent
{
    private readonly TType type;
    private readonly TName name;

    public Parameter(TType type, TName name)
    {
        this.type = type;
        this.name = name;
    }

    public void Write(Output output)
    {
        output.Write(type);
        output.Write(" ");
        output.Write(name);
    }

    public override string ToString() => this.Print();
}