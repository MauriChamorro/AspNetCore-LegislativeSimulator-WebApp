namespace WebAppMVC.Infrastructure.Interfaces;

public interface ISimulationServices
{
    Task<string> AssignCommissionsFor(int projectId);
    Task<string> DoNextReferringRandomly(int projectId);
}