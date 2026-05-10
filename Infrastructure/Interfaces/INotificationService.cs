using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebAppMVC.Infrastructure.Interfaces;

public interface INotificationService
{
    void SendNotification(ITempDataDictionary tempData);
    void AddCommissionAssignedNotification(int projectId);
    void AddChangedCurrentReferralStateNotification(int projectId);
    void AddSendToSessionNotification(int projectId);
    void AddSessionResultNotification(int projectId, bool success);
    void AddProjectCreatedNotification();
    void AddProjectUpdatedNotification();
    void AddSendToCommissionsNotification();
}