using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Services;

namespace WebAppMVC.Infrastructure.Services;

public class CommissionService: ICommissionService
{
    private List<Commission> _commissions;

    public CommissionService()
    {
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
                    "escuela", "enseñanza", "maestros", "alumnos"
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
}