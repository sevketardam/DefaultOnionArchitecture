using DefaultOnionArchitecture.Application.Interface.UnitOfWorks;
using DefaultOnionArchitecture.Domain.Entities;
using MediatR;

namespace DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Queries.GetMetaTagsByKeys;

public class GetMetaTagsByKeysQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetMetaTagsByKeysQueryRequest, IList<MetaTag>>
{
    public async Task<IList<MetaTag>> Handle(GetMetaTagsByKeysQueryRequest request, CancellationToken cancellationToken)
    {
        var allTags = await unitOfWork.GetReadRepository<MetaTag>()
            .GetAllAsync(x => x.IsActive && x.LanguageId == request.LanguageId);

        return [.. allTags
        .Where(tag =>
        {
            if (string.IsNullOrWhiteSpace(tag.PageKeys))
                return true;

            var keys = tag.PageKeys
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim().ToLower());

             if (request.Url == "" && (keys.Contains("anasayfa") || keys.Contains("home")))
                return true;

            return keys.Contains(request.Url);
        })];
    }
}
