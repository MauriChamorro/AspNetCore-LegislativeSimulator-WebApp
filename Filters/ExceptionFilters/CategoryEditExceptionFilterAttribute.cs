using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAppMVC.Infrastructure.Repositories.StaticRepositories;
using WebAppMVC.Models;

namespace WebAppMVC.Filters.ExceptionFilters;

public class CategoryEditExceptionFilterAttribute: ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);
        
        var category = context.RouteData.Values["category"] as Category;
        if (category != null)
        {
            if (!StaticCategoriesRepositories.Exist(category))
            {
                context.ModelState.AddModelError("category-name", "Category does not exist anymore.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status404NotFound
                };
                context.Result = new NotFoundObjectResult(problemDetails);        
            }
        }
    }
}