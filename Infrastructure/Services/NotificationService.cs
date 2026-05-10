using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WebAppMVC.Domain.Services;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Services;

public class NotificationService: INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IProjectService _projectService;

    public void CheckNotifications(ITempDataDictionary tempData)
    {
        if (ThereAreNotification())
        {
            var noti = GetNextNotification();
            tempData["SwalTitle"] = noti.Title;
            tempData["SwalMessage"] = noti.Message;
            tempData["SwalIcon"] = "info"; // success, error, warning, info
        }
    }
    
    public NotificationService(INotificationRepository notificationRepository,
        IProjectService projectService)
    {
        _notificationRepository = notificationRepository;
        _projectService = projectService;
    }
    
    public void AddCommissionAssignedNotification(int projectId)
    {
        var project = _projectService.GetProjectById(projectId);
        var notificationVm = new NotificationViewModel
        {
            Title = "Comisiones asignadas",
            Message = $"Se asignaron comisiones al proyecto: {project.Title}"
        };

        _notificationRepository.Add(notificationVm);
    }

    private bool ThereAreNotification() => 
        _notificationRepository.GetAll().Count > 0;

    private NotificationViewModel GetNextNotification() 
        => _notificationRepository.GetNext();

    
}