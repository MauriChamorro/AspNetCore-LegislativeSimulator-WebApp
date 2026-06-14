using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Infrastructure.Interfaces;

public interface INotificationService
{
    void SendNotification(ITempDataDictionary tempData);
    Task AddCommissionAssignedNotification(int projectId);
    Task AddReferralChangeNotification(int projectId);
    void AddSentToSessionNotification(string userIdentifier);
    void AddSessionResultNotification(Project projectId, bool success);
    void AddProjectCreatedNotification();
    void AddProjectUpdatedNotification();
    void AddSentToCommissionsNotification();
    Task AddProjectStateChangedNotification(int projectId);
    void AddProjectDeletedNotification(string userIdentifier);
    bool ExistNotificationForCurrentUser();
}