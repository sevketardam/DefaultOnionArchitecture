using System.Web;
using DefaultOnionArchitecture.Application.Interface.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;

namespace DefaultOnionArchitecture.UI.ViewComponents;

public class BreadcrumbJsonLdComponent
: ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var url = HttpContext.Request.Path.Value;
        var segments = url!.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);

        var domain = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        var itemList = new List<object>();

        itemList.Add(new
        {
            @type = "ListItem",
            position = 1,
            name = "Anasayfa",
            item = domain
        });

        string cumulativePath = "";
        for (int i = 0; i < segments.Length; i++)
        {
            cumulativePath += "/" + segments[i];
            itemList.Add(new
            {
                @type = "ListItem",
                position = i + 2,
                name = ToTitleCase(segments[i]),
                item = domain + cumulativePath
            });
        }

        var breadcrumb = new
        {
            @context = "https://schema.org",
            @type = "BreadcrumbList",
            itemListElement = itemList
        };

        var json = System.Text.Json.JsonSerializer.Serialize(breadcrumb);
        ViewData["BreadcrumbJson"] = json;

        return View();
    }

    private string ToTitleCase(string slug)
    {
        return HttpUtility.UrlDecode(slug)
            ?.Replace("-", " ")
            ?.Replace("_", " ")
            ?.Trim()
            ?.Split(' ')
            .Select(word => char.ToUpper(word[0]) + word.Substring(1))
            .Aggregate((a, b) => $"{a} {b}")!;
    }
}
