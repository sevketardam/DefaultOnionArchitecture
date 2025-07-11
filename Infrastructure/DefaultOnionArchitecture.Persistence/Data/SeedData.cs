using DefaultOnionArchitecture.Application.Interface.UnitOfWorks;
using DefaultOnionArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DefaultOnionArchitecture.Persistence.Data;
public static class SeedData
{
    public static async void AddRoles(this IServiceProvider service)
    {
        using (var scope = service.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            if (!await roleManager.RoleExistsAsync("user")) await roleManager.CreateAsync(new Role
            {
                Id = Guid.NewGuid(),
                Name = "user",
                NormalizedName = "USER",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            });

            if (!await roleManager.RoleExistsAsync("admin")) await roleManager.CreateAsync(new Role
            {
                Id = Guid.NewGuid(),
                Name = "admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            });
        }
    }

    public static async void AddLang(this IServiceProvider service)
    {
        await using (var scope = service.CreateAsyncScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var trSettings = await unitOfWork.GetReadRepository<Language>().GetAsync(x => x.Id == 1);
            if (trSettings is null)
            {
                await unitOfWork.GetWriteRepository<Language>().AddAsync(new()
                {
                    Id = 1,
                    LangShort = "tr",
                    Lang = "Türkçe",
                    LangIcon = "/assets/images/flags/turkey.png",
                    IsActive = true
                });
                await unitOfWork.SaveAsync();
            }

            var enSettings = await unitOfWork.GetReadRepository<Language>().GetAsync(x => x.Id == 2);

            if (enSettings is null)
            {
                await unitOfWork.GetWriteRepository<Language>().AddAsync(new()
                {
                    Id = 2,
                    LangShort = "en",
                    Lang = "İngilizce",
                    LangIcon = "/assets/images/flags/usa.png",
                    IsActive = true
                });
                await unitOfWork.SaveAsync();
            }
        }
    }

}
