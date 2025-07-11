using System.Security.Claims;
using DefaultOnionArchitecture.Application.Interface.AutoMapper;
using DefaultOnionArchitecture.Application.Interface.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DefaultOnionArchitecture.Application.Bases;

public class BaseHandler
{
    public readonly IMapper mapper;
    public readonly IUnitOfWork unitOfWork;
    public readonly IHttpContextAccessor httpContextAccessor;
    public readonly Guid userId;

    public BaseHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        this.mapper = mapper;
        this.unitOfWork = unitOfWork;
        this.httpContextAccessor = httpContextAccessor;
        var userIdStr = httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        userId = Guid.TryParse(userIdStr, out var parsedUserId) ? parsedUserId : Guid.Empty;
    }
}
