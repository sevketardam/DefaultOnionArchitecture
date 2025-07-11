using MediatR;

namespace DefaultOnionArchitecture.Application.Features.SEO.Sitemap.Command.UpdateSitemap;

public class UpdateSitemapCommandRequest : IRequest<Unit>
{
    public string Content { get; set; } = string.Empty;
}