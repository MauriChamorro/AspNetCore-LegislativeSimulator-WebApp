using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Repositories.InMemoryRepositories;

public class InMemoryNotificationRepository : INotificationRepository
{
    private readonly List<NotificationViewModel> _notis;

    public InMemoryNotificationRepository()
    {
        _notis = new List<NotificationViewModel>();
    }

    public void Add(NotificationViewModel notificationVm) => _notis.Add(notificationVm);

    public bool ExistNotificationsFor(string userIdentifier) =>
        _notis.Exists(n => n.UserIdentifier == userIdentifier);

    public NotificationViewModel GetNotificationForUser(string userIdentifier) =>
        _notis.Find(n => n.UserIdentifier == userIdentifier)!;

    public void RemoveNotification(NotificationViewModel notificationVm) => 
        _notis.Remove(notificationVm);
}