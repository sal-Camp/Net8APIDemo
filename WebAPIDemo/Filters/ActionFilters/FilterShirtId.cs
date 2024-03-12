using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIDemo.Data;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Filters.ActionFilters;

public class FilterShirtId(ApplicationDbContext db) : ActionFilterAttribute
{
    private readonly ApplicationDbContext _db = db;

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
            else
            {
                var shirt = _db.Shirts.Find(shirtId);

                if (shirt is null)
                {
                    context.ModelState.AddModelError("ShirtId", "Shirt doesn't exist.");
                    ValidationProblemDetails problemDetails = new(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
                else
                {
                    // Return found object up to HttpContext
                    context.HttpContext.Items["shirt"] = shirt;
                }
            }
        }
    }
}