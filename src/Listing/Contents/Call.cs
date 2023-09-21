using Listing.Code;

namespace Listing.Contents;

public readonly struct Call<TMethod, TArguments> : IContent
    where TMethod : IContent
    where TArguments : IContent
{
    private readonly TMethod method;
    private readonly TArguments arguments;

    public Call(TMethod method, TArguments arguments)
    {
        this.method = method;
        this.arguments = arguments;
    }

    public void Write(Output output)
    {
        output.Write(method);
        output.Write(new Enclosed<TArguments>(arguments, Enclosing.Parenthesis));
    }

    public override string ToString() => this.Print();
}