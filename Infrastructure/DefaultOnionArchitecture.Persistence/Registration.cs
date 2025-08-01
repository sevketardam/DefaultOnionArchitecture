﻿using DefaultOnionArchitecture.Application.Interface.Repositories;
using DefaultOnionArchitecture.Application.Interface.UnitOfWorks;
using DefaultOnionArchitecture.Domain.Entities;
using DefaultOnionArchitecture.Persistence.Context;
using DefaultOnionArchitecture.Persistence.Repositories;
using DefaultOnionArchitecture.Persistence.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DefaultOnionArchitecture.Persistence;

public static class Registration
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOpt =>
                {
                    sqlOpt.MigrationsAssembly("DefaultOnionArchitecture.Persistence");
                    sqlOpt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                }));

        services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
        services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();


        services.AddIdentityCore<User>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequiredLength = 2;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireDigit = false;
            opt.SignIn.RequireConfirmedEmail = false;
        }).AddRoles<Role>()
            .AddEntityFrameworkStores<AppDbContext>();


    }
}
