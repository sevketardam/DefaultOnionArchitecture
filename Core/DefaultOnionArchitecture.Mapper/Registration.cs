using Microsoft.Extensions.DependencyInjection;

namespace DefaultOnionArchitecture.Mapper;

public static class Registration
{
    public static void AddCustomMapper(this IServiceCollection services)
    {
        services.AddSingleton<Application.Interface.AutoMapper.IMapper, AutoMapper.Mapper>();
        services.AddAutoMapper(typeof(AutoMapper.Mapper).Assembly);
    }
}
