using AutoMapper;

namespace DefaultOnionArchitecture.Application.Interface.AutoMapper;

public interface IMapper
{
    TDestination Map<TDestination, TSource>(
        TSource source,
        string? ignore = null,
        Action<IMappingExpression<TSource, TDestination>>? specialConfig = null);

    IList<TDestination> Map<TDestination, TSource>(
        IList<TSource> source,
        string? ignore = null,
        Action<IMappingExpression<TSource, TDestination>>? specialConfig = null);

    TDestination Map<TDestination>(
        object source,
        string? ignore = null,
        Action<IMappingExpression<object, TDestination>>? specialConfig = null);

    IList<TDestination> Map<TDestination>(
        IList<object> source,
        string? ignore = null,
        Action<IMappingExpression<object, TDestination>>? specialConfig = null);

    TDestination MapWithProfile<TDestination, TSource>(TSource source,
        params Profile[] profiles);
}
