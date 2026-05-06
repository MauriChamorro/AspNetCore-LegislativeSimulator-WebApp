using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Domain.Services;

namespace WebAppMVC.Infrastructure.Services;

public class CommissionService: ICommissionService
{
    private readonly IAssignedCommissionsByProjectRepository _assignedCommissionsByProjectRepository;
    
    private List<Commission> _commissions; //this is a inmemory repo for now

    public CommissionService(IAssignedCommissionsByProjectRepository assignedCommissionsByProjectRepository)
    {
        _assignedCommissionsByProjectRepository = assignedCommissionsByProjectRepository;
        _commissions = new List<Commission>
        {
            new()
            {
                CommissionId = 1,
                Name = "Medio ambiente",
                WordsForAssingment = new List<string>
                {
                    "glaciar", "glaciares", "perito moreno", "litoral"
                }
            },
            new()
            {
                CommissionId = 2,
                Name = "Educación",
                WordsForAssingment = new List<string>
                {
                    "escuela", "enseñanza", "maestros", "alumnos", "educativa","escolar"
                }
            },
            new()
            {
                CommissionId = 3,
                Name = "Libertad",
                WordsForAssingment = new List<string>
                {
                    "libertad", "libre"
                }
            }
        };
    }
    public List<Commission> EvaluateCommissionFor(string projectArticles)
    {
        var assignedCommissions = new List<Commission>();
        var words = projectArticles.ToLower().Split();
        foreach (var commission in _commissions)
        {
            foreach (var wordInArticles in words)
            {
                if (commission.WordsForAssingment.Contains(wordInArticles))
                {
                    assignedCommissions.Add(commission);
                    break;
                } 
            }
        }
        return assignedCommissions;
    }

    public AssignedCommissions AssignCommissionTo(List<Commission> commissions, int projectId)
    {
        AssignedCommissions assignedCommissions = new()
        {
            ProjectId = projectId,
            Commissions = commissions
        };
        _assignedCommissionsByProjectRepository.AddAssignedCommissions(assignedCommissions);
        return assignedCommissions;
    }

    public bool HasBeenAssigned(int projectId)
    {
        return _assignedCommissionsByProjectRepository.ExistProjectId(projectId);
    }
    
    public AssignedCommissions GetAssignedCommissionsFor(int projectId) => 
        _assignedCommissionsByProjectRepository.GetCommissionsFor(projectId);
}