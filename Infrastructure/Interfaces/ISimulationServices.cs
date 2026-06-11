namespace WebAppMVC.Infrastructure.Interfaces;

public interface ISimulationServices
{
    Task<string> AssignCommissionsFor(int projectId);
}