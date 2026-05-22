using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAppMVC.Filters.ExceptionFilters;

public class GlobalExceptionFilter: IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }
    
    public void OnException(ExceptionContext context)
    {
        var env = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
        var expMessage = "";
        
        _logger.LogError(context.Exception.Message,  context.Exception.StackTrace, "Ocurrió un error no controlado en la petición: {Path}", context.HttpContext.Request.Path);

        if (env.IsDevelopment())
        {
            expMessage = context.Exception.Message;
            expMessage += "\n";
            expMessage += context.Exception.StackTrace;
        }
        context.Result = new RedirectToActionResult("Error", "Home",
            new { bodyMessage = expMessage });
        context.ExceptionHandled = true;
    }
}