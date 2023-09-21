using Listing.Code;

namespace Listing.Contents;

public readonly struct Verbatim : IContent
{
    private readonly string value;

    public Verbatim(string value)
    {
        this.value = value;
    }

    public void Write(Output output)
    {
        output.Write(value);
    }

    public static implicit operator Verbatim(string value) => new(value);

    public override string ToString() => this.Print();
}