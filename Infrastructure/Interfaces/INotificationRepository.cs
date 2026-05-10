using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Interfaces;

public interface INotificationRepository
{
    void Add(NotificationViewModel notificationVm);
    List<NotificationViewModel> GetAll();
    NotificationViewModel GetNext();
}