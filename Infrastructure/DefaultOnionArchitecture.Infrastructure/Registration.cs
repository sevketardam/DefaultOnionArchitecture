using System.Text;
using DefaultOnionArchitecture.Application.Interface.ExceptionLogger;
using DefaultOnionArchitecture.Application.Interface.FileUpload;
using DefaultOnionArchitecture.Application.Interface.RedisCache;
using DefaultOnionArchitecture.Application.Interface.SEO;
using DefaultOnionArchitecture.Application.Interface.Tokens;
using DefaultOnionArchitecture.Infrastructure.FileUpload;
using DefaultOnionArchitecture.Infrastructure.Logger;
using DefaultOnionArchitecture.Infrastructure.RedisCache;
using DefaultOnionArchitecture.Infrastructure.Services;
using DefaultOnionArchitecture.Infrastructure.Tokens;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DefaultOnionArchitecture.Infrastructure;

public static class Registration
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TokenSettings>(configuration.GetSection("JWT"));
        services.AddTransient<ITokenService, TokenService>();

        services.Configure<RedisCacheSettings>(configuration.GetSection("RedisCacheSettings"));
        services.AddTransient<IRedisCacheService, RedisCacheService>();
        services.AddTransient(typeof(IFileService), typeof(FileService));
        services.AddTransient<IExceptionLogger, TxtExceptionLogger>();
        services.AddTransient<IRobotsTxtFileService, RobotsTxtFileService>();
        services.AddTransient<ISitemapService, SitemapService>();

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddStackExchangeRedisCache(opt =>
        { 
            opt.Configuration = configuration["RedisCacheSettings:ConnectionString"];
            opt.InstanceName = configuration["RedisCacheSettings:InstanceName"];
        });
    }
}
