using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Interfaces;

public interface INotificationService
{
    void AddCommissionAssignedNotification(int projectId);
    bool ThereAreNotification();
    NotificationViewModel GetNextNotification();
}