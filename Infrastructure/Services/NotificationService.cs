using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Services;

public class NotificationService: INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }
    public void AddCommissionAssignedNotification(int projectId)
    {
        var noti = new NotificationViewModel()
        {
            Title = "Comisiones asignadas",
            Message = $"Comisiones asignadas {projectId}"
        };

        _notificationRepository.Add(noti);

    }

    public bool ThereAreNotification() => 
        _notificationRepository.GetAll().Count > 0;

    public NotificationViewModel GetNextNotification()
    {
        return _notificationRepository.GetNext();
    }
}