using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Repositories.InMemoryRepositories;

public class InMemoryNotificationRepository: INotificationRepository
{
    private readonly Queue<NotificationViewModel> _notis;

    public InMemoryNotificationRepository()
    {
        _notis =  new Queue<NotificationViewModel>();
    }
    public void Add(NotificationViewModel notificationVm) => _notis.Enqueue(notificationVm);

    public List<NotificationViewModel> GetAll() => _notis.ToList();
    
    public NotificationViewModel GetNext() => _notis.Dequeue();
}