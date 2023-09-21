using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Listing.Contents;

public static class ContentFactory
{
    public static Dotted<T1, T2> Dot<T1, T2>(T1 first, T2 second)
        where T1 : IContent
        where T2 : IContent
        => new(first, second);

    public static Verbatim Verbatim(string value) => new(value);
    public static Type From(ITypeSymbol symbol) => new(symbol);

    public static Parameter<Type, VerbatimPrefixed<Verbatim>> Parameter(IParameterSymbol parameter) => Parameter(
        From(parameter.Type),
        Verbatim(parameter.Name).VerbatimPrefixed()
    );

    public static Parameter<TType, TName> Parameter<TType, TName>(TType type, TName name)
        where TType : IContent
        where TName : IContent
        => new(type, name);

    public static Namespace Declaration(INamespaceSymbol symbol) => new(symbol, globalPrefix: false);

    public static Enclosed<T> Parentheses<T>(T content) where T : IContent => new(content, Enclosing.Parenthesis);


    internal static ParametersArguments ParametersArguments(ImmutableArray<IParameterSymbol> parameters)
        => new(parameters);

    public static Call<T1, T2> Call<T1, T2>(T1 method, T2 arguments)
        where T1 : IContent
        where T2 : IContent
        => new(method, arguments);

    internal static Generic<TType, TParameters> Generic<TType, TParameters>(TType type, TParameters parameters)
        where TType : IContent
        where TParameters : IContent
        => new(type, parameters);

    public static Cast<TTargetType, TArgument> Cast<TTargetType,TArgument>(TTargetType targetType, TArgument argument)
        where TArgument : IContent
        where TTargetType : IContent
        => new(targetType, argument);

    public static SafeCast<TArgument, TTargetType> SafeCast<TArgument, TTargetType>(TArgument argument, TTargetType targetType)
        where TArgument : IContent
        where TTargetType : IContent
        => new(argument, targetType);
}