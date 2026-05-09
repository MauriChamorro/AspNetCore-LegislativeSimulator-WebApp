using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAppMVC.Filters.ExceptionFilters;

public class UnexpectedHandleException: ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        context.Result = new RedirectToActionResult("Error", "Home", null);
        context.ExceptionHandled = true;
    }
}