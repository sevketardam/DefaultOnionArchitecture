using MediatR;

namespace DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Queries.GetMetaTagTable;

public class GetMetaTagTableQueryRequest : IRequest<IList<GetMetaTagTableQueryResponse>>
{
    public int? Lang { get; set; }
}
