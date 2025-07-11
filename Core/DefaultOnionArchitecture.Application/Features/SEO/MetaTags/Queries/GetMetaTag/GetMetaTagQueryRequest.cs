using MediatR;

namespace DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Queries.GetMetaTag;

public class GetMetaTagQueryRequest(int id) : IRequest<GetMetaTagQueryResponse>
{
    public int Id { get; set; } = id;
}
