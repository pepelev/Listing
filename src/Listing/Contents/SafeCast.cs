using Listing.Code;

namespace Listing.Contents;

public readonly struct SafeCast<TArgument, TTargetType> : IContent
    where TArgument : IContent
    where TTargetType : IContent
{
    private readonly TArgument argument;
    private readonly TTargetType targetType;

    public SafeCast(TArgument argument, TTargetType targetType)
    {
        this.argument = argument;
        this.targetType = targetType;
    }

    public void Write(Output output)
    {
        output.Write(argument);
        output.Write(" is ");
        output.Write(targetType);
        output.Write(" ? ");
        output.Write(new Cast<TArgument, TTargetType>(argument, targetType));
        output.Write(" : default");
    }

    public override string ToString() => this.Print();
}