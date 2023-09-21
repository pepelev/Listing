using Listing.Code;

namespace Listing.Contents;

internal readonly struct Generic<TMethod, TParameters> : IContent
    where TMethod : IContent
    where TParameters : IContent
{
    private readonly TMethod method;
    private readonly TParameters parameters;

    public Generic(TMethod method, TParameters parameters)
    {
        this.method = method;
        this.parameters = parameters;
    }

    public void Write(Output output)
    {
        output.Write(method);
        output.Write(new Enclosed<TParameters>(parameters, Enclosing.Generic));
    }

    public override string ToString() => this.Print();
}