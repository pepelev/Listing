using System.Text;
using Listing.Contents;

namespace Listing.Code;

public sealed class Output
{
    internal readonly Block block;
    private readonly Line line;
    private int indent = 0;
    private bool started = false;
    private readonly StringBuilder buffer;

    public Output(StringBuilder buffer)
    {
        this.buffer = buffer;
        block = new Block(this);
        line = new Line(this);
    }

    public Block OpenBlock()
    {
        buffer
            .Append('\t', indent)
            .AppendLine("{");
        indent++;
        started = false;
        return block;
    }

    private void CloseBlock()
    {
        indent--;
        buffer
            .Append('\t', indent)
            .AppendLine("}");
        started = false;
    }

    public Region OpenRegion(string name)
    {
        Write("#region ");
        Write(name);
        EndLine();
        return new Region(this);
    }

    private void CloseRegion()
    {
        Write("#endregion");
        EndLine();
    }

    public Block OpenNamespace<T>(T content) where T : IContent
    {
        buffer
            .Append('\t', indent)
            .Append("namespace ");
        content.Write(this);
        buffer.AppendLine();
        return OpenBlock();
    }

    public void Write(string content)
    {
        if (!started)
        {
            buffer.Append('\t', indent);
            started = true;
        }

        buffer.Append(content);
    }

    public void Write<T>(in T content) where T : IContent => content.Write(this);

    public void TryWrite<T>(T? content) where T : struct, IContent
    {
        if (content is { } value)
        {
            Write(value);
        }
    }

    public Line StartLine() => line;

    public void WriteLine<T>(in T content) where T : IContent
    {
        content.Write(this);
        EndLine();
    }

    private void EndLine()
    {
        buffer.AppendLine();
        started = false;
    }

    // [MustUseReturnValue]
    public SeparatedList CommaSeparated() => Separated(", ");

    // [MustUseReturnValue]
    public SeparatedList Separated(string separator) => new(this, separator);

    public void Clear()
    {
        buffer.Clear();
    }

    public override string ToString() => buffer.ToString();

    public readonly struct Region : IDisposable
    {
        private readonly Output output;

        public Region(Output output)
        {
            this.output = output;
        }

        public void Dispose()
        {
            output.CloseRegion();
        }
    }

    public sealed class Block : IDisposable
    {
        private readonly Output output;

        public Block(Output output)
        {
            this.output = output;
        }

        public void Dispose()
        {
            output.CloseBlock();
        }
    }

    public sealed class Line : IDisposable
    {
        private readonly Output output;

        public Line(Output output)
        {
            this.output = output;
        }

        public void Dispose()
        {
            output.EndLine();
        }
    }

    public struct SeparatedList
    {
        private bool empty = true;
        private readonly string separator;
        private readonly Output output;

        public SeparatedList(Output output, string separator)
        {
            this.output = output;
            this.separator = separator;
        }

        public void Append<T>(in T content) where T : IContent
        {
            if (!empty)
            {
                output.Write(separator);
            }

            output.Write(content);
            empty = false;
        }

        public void TryAppend<T>(T? content) where T : struct, IContent
        {
            if (content is { } value)
            {
                Append(value);
            }
        }
    }
}