using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Filters.ExceptionFilters;

public class HandleUpdateExceptions : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);

        var strShirtId = context.RouteData.Values["id"] as string;
        if (int.TryParse(strShirtId, out int shirtId))
        {
            if (!ShirtRepository.ShirtExists(shirtId))
            {
                context.ModelState.AddModelError("ShirtId", "ShirtId does not exist.");
                ValidationProblemDetails problemDetails = new(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);
            }
        }
    }
}