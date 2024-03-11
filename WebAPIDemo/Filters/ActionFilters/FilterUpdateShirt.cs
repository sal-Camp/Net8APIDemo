using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIDemo.Models;

namespace WebAPIDemo.Filters;

public class FilterUpdateShirt : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        var id = context.ActionArguments["id"] as int?;
        var shirt = context.ActionArguments["shirt"] as Shirt;

        if (id.HasValue && shirt is not null && id != shirt.ShirtId)
        {
            context.ModelState.AddModelError("ShirtId", "ShirtId is not the same as Id");
            ValidationProblemDetails problemDetails = new(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }
}