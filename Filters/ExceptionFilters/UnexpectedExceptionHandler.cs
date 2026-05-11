using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAppMVC.Filters.ExceptionFilters;

public class UnexpectedExceptionHandler: ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        context.Result = new RedirectToActionResult("Error", "Home", null);
        //logger for production
        context.ExceptionHandled = true;
    }
}