using Listing.Code;

namespace Listing.Contents;

public readonly struct Enclosed<T> : IContent
    where T : IContent
{
    private readonly T content;
    private readonly Enclosing enclosing;

    public Enclosed(T content, Enclosing enclosing)
    {
        this.content = content;
        this.enclosing = enclosing;
    }

    public void Write(Output output)
    {
        output.Write(enclosing.Left);
        content.Write(output);
        output.Write(enclosing.Right);
    }

    public override string ToString() => this.Print();
}