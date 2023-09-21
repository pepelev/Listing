using Listing.Code;

namespace Listing.Contents;

public readonly struct Dotted<T1, T2> : IContent
    where T1 : IContent
    where T2 : IContent
{
    private readonly T1 first;
    private readonly T2 second;

    public Dotted(T1 first, T2 second)
    {
        this.first = first;
        this.second = second;
    }

    public void Write(Output output)
    {
        output.Write(first);
        output.Write(".");
        output.Write(second);
    }

    public override string ToString() => this.Print();
}