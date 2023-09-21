using Listing.Code;

namespace Listing.Contents;

public readonly struct VerbatimPrefixed<T> : IContent
    where T : IContent
{
    private readonly T content;

    public VerbatimPrefixed(T content)
    {
        this.content = content;
    }

    public void Write(Output output)
    {
        output.Write("@");
        content.Write(output);
    }

    public override string ToString() => this.Print();
}