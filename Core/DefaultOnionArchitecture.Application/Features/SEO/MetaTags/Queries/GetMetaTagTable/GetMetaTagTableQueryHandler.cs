using DefaultOnionArchitecture.Application.Interface.AutoMapper;
using DefaultOnionArchitecture.Application.Interface.UnitOfWorks;
using DefaultOnionArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Queries.GetMetaTagTable;

public class GetMetaTagTableQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    : IRequestHandler<GetMetaTagTableQueryRequest, IList<GetMetaTagTableQueryResponse>>
{
    public async Task<IList<GetMetaTagTableQueryResponse>> Handle(GetMetaTagTableQueryRequest request, CancellationToken cancellationToken)
    {
        var metaTags = await unitOfWork.GetReadRepository<MetaTag>()
            .GetAllAsync(orderBy: a => a.OrderByDescending(x => x.CreatedDate), include: a => a.Include(b => b.Language));

        if (request.Lang.HasValue)
            metaTags = metaTags.Where(x => x.LanguageId == request.Lang.Value).ToList();

        var map = mapper.Map<GetMetaTagTableQueryResponse, MetaTag>(metaTags, specialConfig: map =>
        {
            map.ForMember(dest => dest.CreatedDate,
                          opt => opt.MapFrom(src => src.CreatedDate.ToString("dd.MM.yyyy")));

            map.ForMember(dest => dest.Language,
                          opt => opt.MapFrom(src => src.Language.LangShort));
        });

        return map;
    }
}
