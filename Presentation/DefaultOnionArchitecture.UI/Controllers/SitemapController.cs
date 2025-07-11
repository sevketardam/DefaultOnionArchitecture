using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;
using DefaultOnionArchitecture.Application.DTOs;
using DefaultOnionArchitecture.Application.Features.SEO.Robots.Command;
using DefaultOnionArchitecture.Application.Features.SEO.Sitemap.Command.UpdateSitemap;
using DefaultOnionArchitecture.Application.Interface.SEO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DefaultOnionArchitecture.UI.Controllers;

public class SitemapController(ISitemapService sitemapService,IMediator mediator) : Controller
{
    [Route("sitemap"), HttpGet]
    public async Task<IActionResult> Index()
        => View(model:await sitemapService.GetSitemapAsync());

    [Route("api/[controller]/[action]"), HttpPost]
    public async Task<IActionResult> Update(SitemapDto dto)
    {
        await mediator.Send(new UpdateSitemapCommandRequest { Content = dto.Content });
        return Ok(new { result = 1 });
    }

    [Route("sitemap.xml"), HttpGet]
    public async Task<IActionResult> Get()
    {
        var items = await sitemapService.GetSitemapItemsAsync();

        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var sitemap = new XElement(ns + "urlset",
            from item in items
            select new XElement(ns + "url",
                new XElement(ns + "loc", item.Loc),
                new XElement(ns + "lastmod", item.LastMod.ToString("yyyy-MM-dd")),
                new XElement(ns + "changefreq", item.ChangeFreq),
                new XElement(ns + "priority", item.Priority.ToString("F1", CultureInfo.InvariantCulture))
            )
        );

        var xml = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), sitemap);
        var xmlString = xml.ToString();

        return Content(xmlString, "application/xml");
    }
}
