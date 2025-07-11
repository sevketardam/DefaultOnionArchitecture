using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace DefaultOnionArchitecture.UI.Filters;

public class CanonicalUrlFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var request = context.HttpContext.Request;
        var canonicalUrl = $"{request.Scheme}://{request.Host}{request.Path}";

        if (context.Controller is Controller controller)
        {
            controller.ViewData["CanonicalUrl"] = canonicalUrl;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
