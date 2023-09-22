using System.Collections.Immutable;
using Listing.Code;
using Microsoft.CodeAnalysis;

namespace Listing.Contents;

internal readonly struct ParametersArguments : IContent
{
    private readonly ImmutableArray<IParameterSymbol> parameters;

    public ParametersArguments(ImmutableArray<IParameterSymbol> parameters)
    {
        this.parameters = parameters;
    }

    public void Write(Output output)
    {
        var arguments = output.CommaSeparated();
        foreach (var parameter in parameters)
        {
            arguments.WriteItem(ContentFactory.Verbatim(parameter.Name));
        }
    }

    public override string ToString() => this.Print();
}