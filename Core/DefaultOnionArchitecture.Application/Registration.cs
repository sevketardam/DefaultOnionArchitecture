using System.Reflection;
using DefaultOnionArchitecture.Application.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Globalization;
using MediatR;
using DefaultOnionArchitecture.Application.Beheviors;
using DefaultOnionArchitecture.Application.Bases;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace DefaultOnionArchitecture.Application;

public static class Registration
{
    public static void AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddTransient<ExceptionMiddleware>();
        services.AddRulesFromAssemblyContainer(assembly, typeof(BaseRules));

        services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly);

        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("tr");

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehevior<,>));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RedisCacheBehevior<,>));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        }).AddCookie(opt =>
        {
            opt.SlidingExpiration = true;
            opt.ExpireTimeSpan = TimeSpan.FromDays(365);
            opt.AccessDeniedPath = "/access-denied";
            opt.LoginPath = "/login";
        });

    }

    private static IServiceCollection AddRulesFromAssemblyContainer(
        this IServiceCollection services,
        Assembly assembly,
        Type type)
    {
        var types = assembly.GetTypes().Where(a => a.IsSubclassOf(type) && type != a).ToList();
        foreach (var item in types)
            services.AddTransient(item);

        return services;
    }
}
