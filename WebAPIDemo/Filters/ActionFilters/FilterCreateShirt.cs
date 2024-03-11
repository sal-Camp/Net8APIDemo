using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIDemo.Models;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Filters;

public class FilterCreateShirt : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        var shirt = context.ActionArguments["shirt"] as Shirt;

        if (shirt is null)
        {
            context.ModelState.AddModelError("Shirt", "Shirt object is null");
            ValidationProblemDetails problemDetails = new(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else
        {
            var existingShirt =
                ShirtRepository.GetShirtByProperties(shirt.Brand, shirt.Gender, shirt.Color, shirt.Size);
            if (existingShirt is not null)
            {
                context.ModelState.AddModelError("Shirt", "Shirt already exists.");
                ValidationProblemDetails problemDetails = new(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }

        }

    }
}