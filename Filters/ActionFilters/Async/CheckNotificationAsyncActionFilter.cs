using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAppMVC.Infrastructure.Interfaces;

namespace WebAppMVC.Filters.ActionFilters.Async;

public class CheckNotificationAsyncActionFilter : IAsyncActionFilter
{
    private readonly INotificationService _notificationService;

    public CheckNotificationAsyncActionFilter(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if(_notificationService.ExistNotificationForCurrentUser())
        {
            var controller = context.Controller as Controller;
            _notificationService.SendNotification(controller.TempData);
        }
        
        await next();
    }
}