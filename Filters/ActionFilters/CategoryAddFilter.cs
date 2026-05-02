using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAppMVC.Infrastructure.Repositories.StaticRepositories;
using WebAppMVC.Models;

namespace WebAppMVC.Filters.ActionFilters;

public class CategoryAddFilter: ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        var newCategory = (Category)context.ActionArguments["category"]!;
        if (newCategory == null)
        {
            context.ModelState.AddModelError("category", "Category is required");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new BadRequestObjectResult(problemDetails);
        }
        else if (StaticCategoriesRepositories.Exist(newCategory))
        {
            context.ModelState.AddModelError("category", "Category already exists");
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
            context.Result = new ObjectResult(problemDetails);
        }
    }
}