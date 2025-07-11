using System.Threading.Tasks;
using DefaultOnionArchitecture.Application.Interface.UnitOfWorks;
using DefaultOnionArchitecture.Domain.Entities;
using DefaultOnionArchitecture.UI.Model.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DefaultOnionArchitecture.UI.Controllers;


public class HomeController(IUnitOfWork unitOfWork, IMediator mediator, UserManager<User> userManager) : Controller
{
    [Route("/"), HttpGet]
    public async Task<IActionResult> Index()
        => View();

}
