using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.IdentityModel.Tokens;
using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Services;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IProjectService _projectService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ClaimsPrincipal _user;

    public NotificationService(INotificationRepository notificationRepository,
        IProjectService projectService,
        IHttpContextAccessor contextAccessor)
    {
        _notificationRepository = notificationRepository;
        _projectService = projectService;
        _contextAccessor = contextAccessor;
        _user = _contextAccessor.HttpContext?.User!;
    }

    public bool ExistNotificationForCurrentUser() =>
        _notificationRepository.ExistNotificationsFor(GetCurrentUserIdentifier());

    public void SendNotification(ITempDataDictionary tempData)
    {
        var notificationVm = GetNotificationForCurrentUser();
        tempData["notification"] = JsonSerializer.Serialize(notificationVm);
        _notificationRepository.RemoveNotification(notificationVm);
    }

    private string GetCurrentUserIdentifier() =>
        _user.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

    public async Task AddCommissionAssignedNotification(int projectId, string toUserIdentifier, string message)
    {
        var project = await GetProjectByIdAsync(projectId);
        var notificationVm = new NotificationViewModel
        {
            UserIdentifier = toUserIdentifier,
            Title = "Comisiones asignadas",
            Message = message.IsNullOrEmpty() ? $"Se asignaron comisiones al proyecto: [{project.Title}]" : message
        };
        AddNotification(notificationVm);
    }

    public async Task AddReferralChangeNotification(int projectId, string toUserIdentifier, string message)
    {
        var project = await GetProjectByIdAsync(projectId);
        var notificationVm = new NotificationViewModel
        {
            UserIdentifier = toUserIdentifier,
            Title = "Cambio de Giro",
            Message = message.IsNullOrEmpty() ? $"El proyecto [{project.Title}] tuvo un cambio en sus Giros de Comisiones" : message
        };
        AddNotification(notificationVm);
    }

    public void AddSentToSessionNotification(string userIdentifier)
    {
        var notificationVm = new NotificationViewModel
        {
            UserIdentifier = userIdentifier,
            Title = "Proyecto Enviado a Sesión"
        };
        AddNotification(notificationVm);
    }

    public async Task AddSessionResultNotification(int projectId, string toUserIdentifier, string message)
    {
        var project = await GetProjectByIdAsync(projectId);
        
        var notificationVm = new NotificationViewModel
        {
            UserIdentifier = toUserIdentifier,
            Title = $"Proyecto [{project.Title}] Dicaminado",
            Message = message
        };
        AddNotification(notificationVm);
    }

    public void AddProjectCreatedNotification(string toUserIdentifier)
    {
        var notificationVm = new NotificationViewModel
        {
            UserIdentifier = toUserIdentifier,
            Title = "Proyecto creado"
        };
        AddNotification(notificationVm);
    }

    public void AddProjectUpdatedNotification(string toUserIdentifier)
    {
        var notificationVm = new NotificationViewModel
        {
            UserIdentifier = toUserIdentifier,
            Title = "Proyecto actualizado"
        };
        AddNotification(notificationVm);
    }

    public void AddSentToCommissionsNotification(string toUserIdentifier)
    {
        var notificationVm = new NotificationViewModel
        {
            UserIdentifier = toUserIdentifier,
            Title = "Proyecto enviado a Comisiones"
        };
        AddNotification(notificationVm);
    }

    public void AddProjectDeletedNotification(string userIdentifier)
    {
        var notificationVm = new NotificationViewModel
        {
            UserIdentifier = userIdentifier,
            Title = "Proyecto eliminado",
        };
        AddNotification(notificationVm);
    }

    private NotificationViewModel GetNotificationForCurrentUser()
        => _notificationRepository.GetNotificationForUser(GetCurrentUserIdentifier());

    private async Task<Project> GetProjectByIdAsync(int projectId) =>
        await _projectService.GetProjectByIdAsync(projectId);

    private void AddNotification(NotificationViewModel notificationVm) =>
        _notificationRepository.Add(notificationVm);
}