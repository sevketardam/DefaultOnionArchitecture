using DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Queries.GetMetaTagsByKeys;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DefaultOnionArchitecture.UI.ViewComponents;

public class MetaTagComponent(IMediator mediator)
    : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var path = HttpContext?.Request?.Path.Value ?? "/";
        var cleanPath = path.Trim('/').ToLower();

        var metaTags = await mediator.Send(new GetMetaTagsByKeysQueryRequest(cleanPath));

        return View(metaTags);
    }
}
