using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Repositories;

public interface IAssignedCommissionsByProjectRepository
{
    void AddAssignedCommissions(AssignedCommissions assignedCommissions);
    AssignedCommissions GetCommissionsFor(int projectId);
}