using DefaultOnionArchitecture.Application.Interface.AutoMapper;
using DefaultOnionArchitecture.Application.Interface.UnitOfWorks;
using DefaultOnionArchitecture.Domain.Entities;
using MediatR;

namespace DefaultOnionArchitecture.Application.Features.Languages.Queries.GetAllLang;

public class GetAllLangQueryHandler : IRequestHandler<GetAllLangQueryRequest, IList<GetAllLangQueryResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetAllLangQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<IList<GetAllLangQueryResponse>> Handle(GetAllLangQueryRequest request, CancellationToken cancellationToken)
    {
        var langs = await unitOfWork.GetReadRepository<Language>().GetAllAsync(a => !a.IsDeleted);
        if (request.GetActive)
            langs = langs.Where(a => a.IsActive).ToList();

        var map = mapper.Map<GetAllLangQueryResponse, Language>(langs);

        return map;
    }
}
