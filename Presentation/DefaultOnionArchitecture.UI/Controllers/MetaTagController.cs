using DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Command.CreateMetaTag;
using DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Command.DeleteMetaTag;
using DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Command.UpdateMetaTag;
using DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Queries.GetMetaTag;
using DefaultOnionArchitecture.Application.Features.SEO.MetaTags.Queries.GetMetaTagTable;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DefaultOnionArchitecture.UI.Controllers;

[Authorize(Roles = "admin")]
public class MetaTagController(IMediator mediator) : Controller
{
    [Route("meta-tag"), HttpGet]
    public IActionResult Index()
        => View();

    [Route("api/[controller]/[action]"), HttpGet]
    public async Task<IActionResult> Table(GetMetaTagTableQueryRequest request)
    {
        var response = await mediator.Send(request);
        return Ok(new
        {
            result = 1,
            data = response
        });
    }

    [Route("api/[controller]/[action]/{id}"), HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        var response = await mediator.Send(new GetMetaTagQueryRequest(id));
        return Ok(new
        {
            result = 1,
            metaTag = response
        });
    }

    [Route("api/[controller]/[action]"), HttpPost]
    public async Task<IActionResult> Create(CreateMetaTagCommandRequest request)
    {
        _ = await mediator.Send(request);
        return Ok(new
        {
            result = 1
        });
    }

    [Route("api/[controller]/[action]"), HttpPost]
    public async Task<IActionResult> Update(UpdateMetaTagCommandRequest request)
    {
        _ = await mediator.Send(request);
        return Ok(new
        {
            result = 1
        });
    }

    [Route("api/[controller]/[action]"), HttpPost]
    public async Task<IActionResult> Delete(DeleteMetaTagCommandRequest request)
    {
        _ = await mediator.Send(request);
        return Ok(new
        {
            result = 1
        });
    }
}
