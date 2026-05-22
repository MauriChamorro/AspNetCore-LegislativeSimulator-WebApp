using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAppMVC.Domain.Services;

namespace WebAppMVC.Filters.ActionFilters.Async;

public class CanDeleteProjectAsyncFilterAttribute: IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var projectService = context.HttpContext.RequestServices.GetService<IProjectService>();
        var projectId = (int)(context.ActionArguments["projectId"] ?? 0);
        var canDelete = await projectService.CanDelete(projectId);
        
        if (!canDelete) 
            context.Result = new RedirectToActionResult("Error",
                "Home", 
                new { errorMessage = "No es posible borrar el proyecto actual" });

        await next();
    }
}