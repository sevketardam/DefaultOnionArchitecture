using DefaultOnionArchitecture.UI.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DefaultOnionArchitecture.UI.Controllers;


public class ErrorController : Controller
{
    [Route("error"), HttpGet]
    public IActionResult Index(string code,int status)
        => View(new ErrorViewModel
        {
            Code = code,
            Status = status
        });

    [Route("not-found"), HttpGet]
    public new IActionResult NotFound()
        => View();

    [Route("access-denied"), HttpGet]
    public IActionResult AccessDenied()
        => View();
}
