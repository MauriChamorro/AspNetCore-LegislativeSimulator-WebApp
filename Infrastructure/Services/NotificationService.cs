using WebAppMVC.Domain.Services;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Services;

public class NotificationService: INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IProjectService _projectService;

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

    public bool ThereAreNotification() => 
        _notificationRepository.GetAll().Count > 0;

    public NotificationViewModel GetNextNotification()
    {
        return _notificationRepository.GetNext();
    }
}