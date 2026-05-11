using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAppMVC.Domain.Services;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Filters.ActionFilters;

public class ProjectVmFilterAttribute: ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        
        var projectService = context.HttpContext.RequestServices.GetService<IProjectService>();
        var projectVm = (ProjectViewModel)context.ActionArguments["projectVm"];

        if (projectVm != null)
        {
            if(!projectService.ExistProject(projectVm.ProjectId))
            {
                context.Result = new RedirectToActionResult("Error", "Home", new { errorMessage = "Proyecto no econtrado" });
            }
            else
            {
                var savedProject = projectService.GetProjectById(projectVm.ProjectId);
                if(!projectService.CanSendToCommission(savedProject))
                    context.Result = new RedirectToActionResult("Error", "Home", new { errorMessage = "No es posible enviar a comisión" });
            }
        }
    }
}