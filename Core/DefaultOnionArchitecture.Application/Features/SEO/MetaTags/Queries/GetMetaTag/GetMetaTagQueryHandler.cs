using DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Rules;
using DefaultOnionArchitecture.Application.Interface.AutoMapper;
using DefaultOnionArchitecture.Application.Interface.UnitOfWorks;
using DefaultOnionArchitecture.Domain.Entities;
using MediatR;

namespace DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Queries.GetMetaTag;

public class GetMetaTagQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, MetaTagRules metaTagRules) : IRequestHandler<GetMetaTagQueryRequest, GetMetaTagQueryResponse>
{
    public async Task<GetMetaTagQueryResponse> Handle(GetMetaTagQueryRequest request, CancellationToken cancellationToken)
    {
        var metaTag = await unitOfWork.GetReadRepository<MetaTag>()
            .GetAsync(a => a.Id == request.Id);
        await metaTagRules.MetaTagCannotBeNullException(metaTag);

        var response = mapper.Map<GetMetaTagQueryResponse, MetaTag>(metaTag, specialConfig: map =>
        {
            map.ForMember(dest => dest.CreatedDate,
                          opt => opt.MapFrom(src => src.CreatedDate.ToString("dd.MM.yyyy")));
        });

        return response;
    }
}
