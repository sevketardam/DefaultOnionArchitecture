using MediatR;

namespace DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Command.DeleteMetaTag;

public class DeleteMetaTagCommandRequest : IRequest<Unit>
{
    public int Id { get; set; }
}
