using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Interfaces;

public interface INotificationRepository
{
    void Add(NotificationViewModel notificationVm);
    bool ExistNotificationsFor(string getUserIdentifier);
    NotificationViewModel GetNotificationForUser(string userIdentifier);
    void RemoveNotification(NotificationViewModel notificationVm);
}