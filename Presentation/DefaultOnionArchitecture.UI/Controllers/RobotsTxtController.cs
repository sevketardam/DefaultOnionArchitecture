using DefaultOnionArchitecture.Application.DTOs;
using DefaultOnionArchitecture.Application.Features.SEO.Robots.Command.UpdateRobotsTxt;
using DefaultOnionArchitecture.Application.Interface.SEO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DefaultOnionArchitecture.UI.Controllers;

[Authorize(Roles = "admin")]
public class RobotsTxtController(IRobotsTxtFileService robotsService, IMediator mediator) : Controller
{
    [Route("robots-txt"), HttpGet]
    public async Task<IActionResult> Index()
        => View(model: await robotsService.GetAsync());

    [Route("api/[controller]/[action]"), HttpPost]
    public async Task<IActionResult> Update(RobotsTxtDto dto)
    {
        await mediator.Send(new UpdateRobotsTxtCommandRequest { Content = dto.Content });
        return Ok(new { result = 1 });
    }
}
