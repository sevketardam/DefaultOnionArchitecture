using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DefaultOnionArchitecture.Application.Bases;
using DefaultOnionArchitecture.Application.Features.Auth.Rules;
using DefaultOnionArchitecture.Application.Interface.AutoMapper;
using DefaultOnionArchitecture.Application.Interface.Tokens;
using DefaultOnionArchitecture.Application.Interface.UnitOfWorks;
using DefaultOnionArchitecture.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DefaultOnionArchitecture.Application.Features.Auth.Command.RefreshToken;

public class RefreshTokenCommandHandler : BaseHandler, IRequestHandler<RefreshTokenCommandRequest, RefreshTokenCommandResponse>
{
    private readonly AuthRules authRules;
    private readonly UserManager<User> userManager;
    private readonly ITokenService tokenService;

    public RefreshTokenCommandHandler(AuthRules authRules, IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, ITokenService tokenService)
        : base(mapper, unitOfWork, httpContextAccessor)
    {
        this.authRules = authRules;
        this.userManager = userManager;
        this.tokenService = tokenService;
    }

    public async Task<RefreshTokenCommandResponse> Handle(RefreshTokenCommandRequest request, CancellationToken cancellationToken)
    {
        var principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        string email = principal.FindFirstValue(ClaimTypes.Email);

        User? user = await userManager.FindByEmailAsync(email);
        IList<string> roles = await userManager.GetRolesAsync(user);

        await authRules.RefreshTokenShouldNotBeExpired(user.RefreshTokenExpiryTime);

        JwtSecurityToken newAccessToken = await tokenService.CreateToken(user,roles);

        string newRefreshToken = tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await userManager.UpdateAsync(user);


        return new()
        {
            RefreshToken = newRefreshToken,
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken)
        };
    }
}