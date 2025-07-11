using DefaultOnionArchitecture.Application.Features.Languages.Queries.GetAllLang;
using DefaultOnionArchitecture.Application.Features.Languages.Queries.LangSelectbox;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DefaultOnionArchitecture.UI.Controllers;

public class LanguageController(IMediator mediator) : Controller
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]"), HttpGet]
    public async Task<IActionResult> Selectlist()
    {
        var response = await mediator.Send(new LangSelectboxQueryRequest());
        return Ok(new
        {
            result = 1,
            data = response
        });
    }

    [AllowAnonymous]
    [Route("api/[controller]/[action]"), HttpGet]
    public async Task<IActionResult> All()
    {
        var response = await mediator.Send(new GetAllLangQueryRequest());
        return Ok(new
        {
            result = 1,
            data = response
        });
    }
}
