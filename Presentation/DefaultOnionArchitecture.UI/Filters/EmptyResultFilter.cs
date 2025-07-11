using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace DefaultOnionArchitecture.UI.Filters;

public class EmptyResultFilter : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult objectResult)
        {
            var value = objectResult.Value;

            var valueString = value?.ToString();

            if (value == null || valueString is "()" ||
                (value is IEnumerable<object> enumerable && !enumerable.Any()))
            {
                context.Result = new OkObjectResult(new { result = 1 });
            }
        }

        base.OnActionExecuted(context);
    }
}
