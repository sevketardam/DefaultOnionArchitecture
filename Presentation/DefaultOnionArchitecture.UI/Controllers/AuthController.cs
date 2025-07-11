using DefaultOnionArchitecture.Application.Features.Auth.Command.Login;
using DefaultOnionArchitecture.Application.Features.Auth.Command.RefreshToken;
using DefaultOnionArchitecture.Application.Features.Auth.Command.Register;
using DefaultOnionArchitecture.Application.Features.Auth.Command.Revoke;
using DefaultOnionArchitecture.Application.Features.Auth.Command.RevokeAll;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DefaultOnionArchitecture.UI.Controllers;

public class AuthController(IMediator mediator) : Controller
{
    [Route("login"), HttpGet]
    public IActionResult Login()
        => HttpContext.User.Identity!.IsAuthenticated ? Redirect("/dashboard") : View();

    [Route("logout"), HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }

    [Route("api/[controller]/[action]"), HttpPost]
    public async Task<IActionResult> Register( RegisterCommandRequest request)
        => Ok(await mediator.Send(request));

    [Route("api/[controller]/[action]"), HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginCommandRequest request)
    {
        var response = await mediator.Send(request);
        return StatusCode(StatusCodes.Status200OK, response);
    }

    [Route("api/[controller]/[action]"), HttpPost]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommandRequest request)
    {
        var response = await mediator.Send(request);
        return StatusCode(StatusCodes.Status200OK, response);
    }


    [Authorize(Roles = "admin")]
    [Route("api/[controller]/[action]"), HttpPost]
    public async Task<IActionResult> Revoke([FromBody] RevokeCommandRequest request)
    {
        await mediator.Send(request);
        return StatusCode(StatusCodes.Status200OK);
    }

    [Authorize(Roles = "admin")]
    [Route("api/[controller]/[action]"), HttpPost]
    public async Task<IActionResult> RevokeAll()
    {
        await mediator.Send(new RevokeAllCommandRequest());
        return StatusCode(StatusCodes.Status200OK);
    }
}
