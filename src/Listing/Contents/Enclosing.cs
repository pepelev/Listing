namespace Listing.Contents;

public readonly struct Enclosing
{
    public static Enclosing Parenthesis => new("(", ")");
    public static Enclosing Generic => new("<", ">");
    public static Enclosing SquareBrackets => new("[", "]");
    public static Enclosing CurlyBrackets => new("{", "}");

    public Enclosing(string left, string right)
    {
        Left = left;
        Right = right;
    }

    public string Left { get; }
    public string Right { get; }

    public override string ToString() => $"{Left} {Right}";
}