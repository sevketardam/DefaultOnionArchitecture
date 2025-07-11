namespace DefaultOnionArchitecture.Application.Interface.RedisCache;

public interface ICacheableQuery
{
    string CacheKey { get; }
    double CacheTime { get; }
}
