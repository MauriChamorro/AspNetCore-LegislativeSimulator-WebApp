using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAppMVC.Domain.Services;

namespace WebAppMVC.Filters.ActionFilters;

public class ReferralCommitteesFilterAttribute: ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
            
        var projectId = (int)(context.ActionArguments["projectId"] ?? 0);
        
        var commissionService = context.HttpContext.RequestServices.GetService<ICommissionService>();

        if (!commissionService.HasBeenAssigned(projectId))
        {
            context.Result = new RedirectToActionResult("Error", "Home", new { errorMessage = "El proyecto no tiene comisiones asignadas" });
        }
            
    }
}