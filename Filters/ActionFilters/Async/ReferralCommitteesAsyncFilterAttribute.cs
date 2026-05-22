using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAppMVC.Domain.Services;

namespace WebAppMVC.Filters.ActionFilters.Async;

public class ReferralCommitteesAsyncFilterAttribute: IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var projectId = (int)(context.ActionArguments["projectId"] ?? 0);
        
        var commissionService = context.HttpContext.RequestServices.GetService<ICommissionService>();
        var hasBeenAssigned = await commissionService.HasBeenAssigned(projectId);
        if (!hasBeenAssigned)
        {
            context.Result = new RedirectToActionResult("Error", "Home", new { errorMessage = "El proyecto no tiene comisiones asignadas" });
        }

        await next();
    }
}