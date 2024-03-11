using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Filters;

public class FilterShirtId : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        if (context.ActionArguments["id"] is int shirtId)
        {
            if (shirtId <= 0)
            {
                context.ModelState.AddModelError("ShirtId", "ShirtId is invalid.");
                ValidationProblemDetails problemDetails = new(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else if (!ShirtRepository.ShirtExists(shirtId))
            {
                context.ModelState.AddModelError("ShirtId", "Shirt doesn't exist.");
                ValidationProblemDetails problemDetails = new(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
        }
    }
}