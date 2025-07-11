using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DefaultOnionArchitecture.Application.Bases;
using DefaultOnionArchitecture.Application.Features.Auth.Rules;
using DefaultOnionArchitecture.Application.Interface.AutoMapper;
using DefaultOnionArchitecture.Application.Interface.Tokens;
using DefaultOnionArchitecture.Application.Interface.UnitOfWorks;
using DefaultOnionArchitecture.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;

namespace DefaultOnionArchitecture.Application.Features.Auth.Command.Login;

public class LoginCommandHandler : BaseHandler, IRequestHandler<LoginCommandRequest, object>
{
    private readonly UserManager<User> userManager;
    private readonly IConfiguration configuration;
    private readonly ITokenService tokenService;
    private readonly AuthRules authRules;

    public LoginCommandHandler(UserManager<User> userManager, IConfiguration configuration, ITokenService tokenService, AuthRules authRules, IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        : base(mapper, unitOfWork, httpContextAccessor)
    {
        this.userManager = userManager;
        this.configuration = configuration;
        this.tokenService = tokenService;
        this.authRules = authRules;
    }

    public async Task<object> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        bool checkPassword = await userManager.CheckPasswordAsync(user, request.Password);

        if (user == null || !checkPassword)
        {
            return new
            {
                result = 2,
                msg = "Kullanıcı adı veya şifre hatalı",
            };
        }

        await authRules.EmailOrPasswordShouldNotBeInvalid(user, checkPassword);

        var roles = await userManager.GetRolesAsync(user);

        JwtSecurityToken token = await tokenService.CreateToken(user, roles);

        

        string refreshToken = tokenService.GenerateRefreshToken();

        _ = int.TryParse(configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

        await userManager.UpdateAsync(user);
        await userManager.UpdateSecurityStampAsync(user);

        var _token = new JwtSecurityTokenHandler().WriteToken(token);

        await userManager.SetAuthenticationTokenAsync(user, "Default", "AccessToken", _token);

        var claims = new List<Claim>();

        foreach (var claim in token.Claims)
            if (claim.Type != ClaimTypes.Email) claims.Add(claim);

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await httpContextAccessor.HttpContext.SignInAsync(
             CookieAuthenticationDefaults.AuthenticationScheme,
             new ClaimsPrincipal(identity),
             new AuthenticationProperties
             {
                 IsPersistent = true,
                 ExpiresUtc = DateTime.UtcNow.AddYears(1)
             });


        return new
        {
            result = 1,
        };

    }
}
