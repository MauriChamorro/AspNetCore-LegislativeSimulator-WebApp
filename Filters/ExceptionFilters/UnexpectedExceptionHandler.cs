using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAppMVC.Filters.ExceptionFilters;

public class UnexpectedExceptionHandler : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var env = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
        var expMessage = "";

        if (env.IsDevelopment())
        {
            expMessage = context.Exception.Message;
            expMessage += "\n";
            expMessage += context.Exception.StackTrace;
        }
        context.Result = new RedirectToActionResult("Error", "Home",
            new { bodyMessage = expMessage });
        //logger for production
        context.ExceptionHandled = true;
    }
}