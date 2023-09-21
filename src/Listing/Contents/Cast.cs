using Listing.Code;

namespace Listing.Contents;

public readonly struct Cast<TTargetType, TArgument> : IContent
    where TTargetType : IContent
    where TArgument : IContent
{
    private readonly TArgument argument;
    private readonly TTargetType targetType;

    public Cast(TTargetType targetType, TArgument argument)
    {
        this.argument = argument;
        this.targetType = targetType;
    }

    public void Write(Output output)
    {
        output.Write(new Enclosed<TTargetType>(targetType, Enclosing.Parenthesis));
        output.Write(argument);
    }

    public override string ToString() => this.Print();
}