using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebAppMVC.Infrastructure.Interfaces;

public interface INotificationService
{
    bool ExistNotificationForCurrentUser();
    void SendNotification(ITempDataDictionary tempData);
    void AddProjectCreatedNotification(string toUserIdentifier);
    void AddProjectUpdatedNotification(string toUserIdentifier);
    void AddProjectDeletedNotification(string userIdentifier);
    void AddSentToCommissionsNotification(string userIdentifier);
    Task AddCommissionAssignedNotification(int projectId, string toUserIdentifier, string message = "");
    Task AddReferralChangeNotification(int projectId, string toUserIdentifier, string message = "");
    void AddSentToSessionNotification(string userIdentifier);
    Task AddSessionResultNotification(int projectId, string toUserIdentifier, string message = "");
}