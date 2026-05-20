using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Infrastructure.Interfaces;

public interface INotificationService
{
    void SendNotification(ITempDataDictionary tempData);
    void AddCommissionAssignedNotification(int projectId);
    void AddReferralChangeNotification(int projectId);
    void AddSendToSessionNotification(Project project);
    void AddSessionResultNotification(int projectId, bool success);
    void AddProjectCreatedNotification();
    void AddProjectUpdatedNotification();
    void AddSentToCommissionsNotification();
    void AddProjectStateChangedNotification(int projectId);
    void AddProjectDeletedNotification();
}