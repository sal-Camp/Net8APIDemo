using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIDemo.Data;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Filters.ExceptionFilters;

public class HandleUpdateExceptions(ApplicationDbContext db) : ExceptionFilterAttribute
{
    private readonly ApplicationDbContext _db = db;
    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);

        var strShirtId = context.RouteData.Values["id"] as string;
        if (int.TryParse(strShirtId, out int shirtId))
        {
            if (_db.Shirts.FirstOrDefault(x => x.ShirtId == shirtId) is null)
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