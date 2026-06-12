using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Infrastructure.Interfaces;

public interface ISimulationServices
{
    Task<string> AssignCommissionsFor(int projectId);
    Task<string> DoNextReferringRandomly(int projectId);
    Task<string> EditReferral(int projectId, int commissionId, ReferralState state);
    Task<string> DoSessionRandomly(int projectId);
    Task<string> DoSession(int projectId, int sessionResult);
}