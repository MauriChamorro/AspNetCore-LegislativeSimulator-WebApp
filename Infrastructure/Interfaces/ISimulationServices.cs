namespace WebAppMVC.Infrastructure.Interfaces;

public interface ISimulationServices
{
    Task AssignCommissionsFor(int projectId);
}