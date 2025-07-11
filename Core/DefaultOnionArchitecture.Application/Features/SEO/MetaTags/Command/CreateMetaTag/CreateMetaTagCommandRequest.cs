using MediatR;

namespace DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Command.CreateMetaTag;

public class CreateMetaTagCommandRequest : IRequest<Unit>
{
    public string? PageKeys { get; set; }
    public string AttributeName { get; set; }
    public string AttributeValue { get; set; }
    public string Content { get; set; }
    public int LanguageId { get; set; }
    public bool IsActive { get; set; }
}
