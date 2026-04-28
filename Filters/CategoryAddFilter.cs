using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAppMVC.Models;
using WebAppMVC.Models.Repositories;

namespace WebAppMVC.Filters;

public class CategoryAddFilter: ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        var newCategory = (Category)context.ActionArguments["category"]!;
        if (newCategory == null)
        {
            context.Result = new BadRequestObjectResult("Category is required");
            context.ModelState.AddModelError("category", "Category is required");
            context.HttpContext.Response.StatusCode = 400;
        }
        else if (StaticCategoriesRepositories.Exist(newCategory))
        {
            context.Result = new ObjectResult("Category already exists");
            context.ModelState.AddModelError("category", "Category already exists");
            context.HttpContext.Response.StatusCode = 200;
        }
    }
}