using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebAppMVC.Infrastructure.Interfaces;

public interface INotificationService
{
    void CheckNotifications(ITempDataDictionary tempData);
    void AddCommissionAssignedNotification(int projectId);
}