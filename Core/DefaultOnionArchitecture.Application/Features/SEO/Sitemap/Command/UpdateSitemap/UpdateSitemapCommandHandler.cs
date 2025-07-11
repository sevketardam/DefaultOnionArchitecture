using DefaultOnionArchitecture.Application.Interface.SEO;
using MediatR;

namespace DefaultOnionArchitecture.Application.Features.SEO.Sitemap.Command.UpdateSitemap;

public class UpdateSitemapCommandHandler(ISitemapService sitemapService) : IRequestHandler<UpdateSitemapCommandRequest, Unit>
{
    public async Task<Unit> Handle(UpdateSitemapCommandRequest request, CancellationToken cancellationToken)
    {
        await sitemapService.UpdateAsync(request.Content);
        return Unit.Value;
    }
}
