using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAppMVC.Domain.Services;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Filters.ActionFilters.Async;

public class ProjectVmAsyncFilterAttribute: IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var projectService = context.HttpContext.RequestServices.GetService<IProjectService>();
        var projectVm = (ProjectViewModel)context.ActionArguments["projectVm"];

        if (projectVm != null)
        { 
            var exist = await projectService.ExistProjectAsync(projectVm.ProjectId);
            if(!exist)
            {
                context.Result = new RedirectToActionResult("Error", "Home", new { errorMessage = "Proyecto no econtrado" });
            }
            else
            {
                var savedProject = await projectService.GetProjectByIdAsync(projectVm.ProjectId);
                if(!projectService.CanSendToCommission(savedProject))
                    context.Result = new RedirectToActionResult("Error", "Home", new { errorMessage = "No es posible enviar a comisión" });
            }
        }
        
        await next();
    }
}