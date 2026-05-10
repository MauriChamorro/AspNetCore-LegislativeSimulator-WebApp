using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAppMVC.Domain.Services;

namespace WebAppMVC.Filters.ActionFilters;

public class ProjectIdNotFoundFilterAttribute: ActionFilterAttribute
{
    
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        
        var projectService = context.HttpContext.RequestServices.GetService<IProjectService>();
        var projectId = (int)(context.ActionArguments["projectId"] ?? 0);
        
        if (!projectService.ExistProject(projectId))
        {
            context.Result = new RedirectToActionResult("Error", "Home", null);
        }
    }
}