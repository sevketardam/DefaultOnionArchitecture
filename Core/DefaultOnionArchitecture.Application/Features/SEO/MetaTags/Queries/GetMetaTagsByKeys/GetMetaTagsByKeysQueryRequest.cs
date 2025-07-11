using DefaultOnionArchitecture.Domain.Entities;
using MediatR;

namespace DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Queries.GetMetaTagsByKeys;

public class GetMetaTagsByKeysQueryRequest(string url, int languageId = 1) : IRequest<IList<MetaTag>>
{
    public string Url { get; } = url;
    public int LanguageId { get; } = languageId;
}