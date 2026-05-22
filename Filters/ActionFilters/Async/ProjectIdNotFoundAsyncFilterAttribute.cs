using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAppMVC.Domain.Services;

namespace WebAppMVC.Filters.ActionFilters.Async;

public class ProjectIdNotFoundAsyncFilterAttribute: IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var projectService = context.HttpContext.RequestServices.GetService<IProjectService>();
        var projectId = (int)(context.ActionArguments["projectId"] ?? 0);
        var exist = await projectService.ExistProjectAsync(projectId);
        if (!exist)
        {
            context.Result = new RedirectToActionResult("Error", "Home", new { errorMessage = "Proyecto no econtrado" });
            return;
        }
        await next();
    }
}