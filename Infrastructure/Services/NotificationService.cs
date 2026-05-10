using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WebAppMVC.Domain.Models.Projects;
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
        var project = GetProjectById(projectId);
        var notificationVm = new NotificationViewModel
        {
            Title = "Comisiones asignadas",
            Message = $"Se asignaron comisiones al proyecto: {project.Title}"
        };
        AddNotification(notificationVm);
    }

    public void AddChangedCurrentReferralStateNotification(int projectId)
    {
        var project = GetProjectById(projectId);
        var notificationVm = new NotificationViewModel
        {
            Title = "Cambio en el Estado de Giro",
            Message = $"El proyecto {project.Title} tuvo un cambio de estado en la comisión actual"
        };
        AddNotification(notificationVm);

    }

    public void AddSendToSessionNotification(int projectId)
    {
        var project = GetProjectById(projectId);
        var notificationVm = new NotificationViewModel
        {
            Title = "Enviado a Sesión",
            Message = $"El proyecto {project.Title} ha sido enviado a sesión"
        };
        AddNotification(notificationVm);
    }

    public void AddSessionResultNotification(int projectId, bool success)
    {
        var project = GetProjectById(projectId);
        var notificationVm = new NotificationViewModel
        {
            Title = "Dictamen",
            Message = $"El proyecto {project.Title} ha sido {GetSessionResultTxt(success)}"
        };
        AddNotification(notificationVm);
    }

    private string GetSessionResultTxt(bool success) => 
        success ? "Aprovado" : "Rechazado";

    private bool ThereAreNotification() => 
        _notificationRepository.GetAll().Count > 0;

    private NotificationViewModel GetNextNotification() 
        => _notificationRepository.GetNext();

    private Project GetProjectById(int projectId) => 
        _projectService.GetProjectById(projectId);

    private void AddNotification(NotificationViewModel notificationVm) => 
        _notificationRepository.Add(notificationVm);
}