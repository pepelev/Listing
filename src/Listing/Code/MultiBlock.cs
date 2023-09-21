namespace Listing.Code;

public readonly struct MultiBlock : IDisposable
{
    private readonly Output output;
    private readonly int times;

    public MultiBlock(Output output, int times)
    {
        this.output = output;
        this.times = times;
    }

    public void Dispose()
    {
        for (var i = 0; i < times; i++)
        {
            output.block.Dispose();
        }
    }
}