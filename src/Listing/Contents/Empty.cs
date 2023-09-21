using Listing.Code;

namespace Listing.Contents;

public readonly struct Empty : IContent
{
    public void Write(Output output)
    {
        // intentionally left blank, it is empty after all
    }

    public override string ToString() => this.Print();
}