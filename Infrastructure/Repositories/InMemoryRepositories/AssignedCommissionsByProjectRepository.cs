using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;

namespace WebAppMVC.Infrastructure.Repositories.InMemoryRepositories;

public class AssignedCommissionsByProjectRepository: IAssignedCommissionsByProjectRepository
{
    private readonly List<AssignedCommissions> _assignedCommissions;

    public AssignedCommissionsByProjectRepository()
    {
        _assignedCommissions = new List<AssignedCommissions>();
    }

    public void AddAssignedCommissions(AssignedCommissions assignedCommissions) => 
        _assignedCommissions.Add(assignedCommissions);

    public AssignedCommissions GetCommissionsFor(int projectId) => 
        _assignedCommissions.First(ac => ac.ProjectId == projectId);
}