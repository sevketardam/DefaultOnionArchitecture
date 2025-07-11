using AutoMapper;
using AutoMapper.Internal;

namespace DefaultOnionArchitecture.Mapper.AutoMapper;

public class Mapper : Application.Interface.AutoMapper.IMapper
{
    public static List<TypePair> typePairs = new();
    private IMapper MapperContainer;

    public TDestination Map<TDestination, TSource>(
        TSource source,
        string? ignore = null,
        Action<IMappingExpression<TSource, TDestination>>? specialConfig = null)
    {
        Config(5, ignore, specialConfig);
        return MapperContainer.Map<TSource, TDestination>(source);
    }

    public IList<TDestination> Map<TDestination, TSource>(
        IList<TSource> source,
        string? ignore = null,
        Action<IMappingExpression<TSource, TDestination>>? specialConfig = null)
    {
        Config(5, ignore, specialConfig);
        return MapperContainer.Map<IList<TSource>, IList<TDestination>>(source);
    }

    public TDestination Map<TDestination>(
        object source,
        string? ignore = null,
        Action<IMappingExpression<object, TDestination>>? specialConfig = null)
    {
        Config(5, ignore, specialConfig);
        return MapperContainer.Map<TDestination>(source);
    }

    public IList<TDestination> Map<TDestination>(
        IList<object> source,
        string? ignore = null,
        Action<IMappingExpression<object, TDestination>>? specialConfig = null)
    {
        Config(5, ignore, specialConfig);
        return MapperContainer.Map<IList<TDestination>>(source);
    }

    public TDestination MapWithProfile<TDestination, TSource>(TSource source, 
        params Profile[] profiles)
    {
        var config = new MapperConfiguration(cfg =>
        {
            foreach (var profile in profiles)
            {
                cfg.AddProfile(profile);
            }
        });

        var mapper = config.CreateMapper();
        return mapper.Map<TDestination>(source);
    }

    protected void Config<TDestination, TSource>(
        int depth = 5,
        string? ignore = null,
        Action<IMappingExpression<TSource, TDestination>>? specialConfig = null)
    {
        var typePair = new TypePair(typeof(TSource), typeof(TDestination));

        if (typePairs.Any(a => a.DestinationType == typePair.DestinationType
                            && a.SourceType == typePair.SourceType)
            && ignore is null && specialConfig is null)
            return;

        typePairs.Add(typePair);

        var config = new MapperConfiguration(config =>
        {
            foreach (var item in typePairs)
            {
                if (item.SourceType == typeof(TSource) && item.DestinationType == typeof(TDestination))
                {
                    var map = config.CreateMap<TSource, TDestination>().MaxDepth(depth);

                    if (!string.IsNullOrWhiteSpace(ignore))
                    {
                        var props = ignore.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                        foreach (var prop in props)
                        {
                            map.ForMember(prop, opt => opt.Ignore());
                        }
                    }

                    specialConfig?.Invoke(map);
                    map.ReverseMap();
                }
                else
                {
                    config.CreateMap(item.SourceType, item.DestinationType).MaxDepth(depth).ReverseMap();
                }
            }
        });

        MapperContainer = config.CreateMapper();
    }

}
