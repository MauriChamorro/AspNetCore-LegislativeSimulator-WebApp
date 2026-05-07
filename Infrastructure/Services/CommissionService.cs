using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Domain.Services;

namespace WebAppMVC.Infrastructure.Services;

public class CommissionService : ICommissionService
{
    private readonly IAssignedCommissionsByProjectRepository _assignedCommissionsByProjectRepository;
    private readonly IReferralCommissionRepository _referralCommissionRepository;

    private List<Commission> _commissions; //this is a inmemory repo for now

    public CommissionService(IAssignedCommissionsByProjectRepository assignedCommissionsByProjectRepository,
        IReferralCommissionRepository referralCommissionRepository)
    {
        _assignedCommissionsByProjectRepository = assignedCommissionsByProjectRepository;
        _referralCommissionRepository = referralCommissionRepository;
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
                    "escuela", "enseñanza", "maestros", "alumnos", "educativa", "escolar"
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
        var referralCommissions = new List<ReferralCommission>();
        foreach (var commission in commissions)
        {
            referralCommissions.Add(
                new ReferralCommission
                {
                    ProjectId = projectId,
                    CommissionId = commission.CommissionId,
                    CommisionName = commission.Name,
                    State = ReferralCommissionState.Assigned,
                    ReferralDate = DateTime.Now
                }
            );
        }

        _referralCommissionRepository.AddRange(referralCommissions);
        return assignedCommissions;
    }

    public bool HasBeenAssigned(int projectId) =>
        _assignedCommissionsByProjectRepository.ExistProjectId(projectId);

    public List<ReferralCommission> GetReferralCommissionsFor(int projectId)
    {
       return  _referralCommissionRepository.GetFor(projectId);
    }

    public ReferralCommission GetActualReferral(List<ReferralCommission> referralCommissions)
    {
        if (referralCommissions.Any(rc => rc.State == ReferralCommissionState.Evaluating))
            return referralCommissions.First(rc => rc.State == ReferralCommissionState.Evaluating);
        return referralCommissions.First(rc => rc.State == ReferralCommissionState.Assigned);
    }

    public void DoNextReferralPhase(ReferralCommission actualReferral)
    {
        if (actualReferral.State == ReferralCommissionState.Assigned)
        {
            actualReferral.State = ReferralCommissionState.Evaluating;
            actualReferral.ReferralDate = DateTime.Now;
        }
        else if (actualReferral.State == ReferralCommissionState.Evaluating)
        {
            actualReferral.State = GetRandomResult();
            actualReferral.ReferralDate = DateTime.Now;
        }
    }

    private static ReferralCommissionState GetRandomResult()
    {
        var rnd = new Random();
        var success = rnd.Next(2) == 0;
        if (success)
            return ReferralCommissionState.Accepted;
        return ReferralCommissionState.Rejected;
    }

    public bool ThereAreNotPendingReferral(List<ReferralCommission> referralCommissions) => 
        referralCommissions.TrueForAll(rc => rc.State == ReferralCommissionState.Accepted ||  rc.State == ReferralCommissionState.Rejected);

    public bool ReferralIsRejected(ReferralCommission actualReferral) =>
        actualReferral.State == ReferralCommissionState.Rejected;

    public bool AcceptedByAllCommission(int projectId) =>
        _referralCommissionRepository.GetFor(projectId)
            .TrueForAll(rc  => rc.State == ReferralCommissionState.Accepted);

    public AssignedCommissions GetAssignedCommissionsFor(int projectId) =>
        _assignedCommissionsByProjectRepository.GetCommissionsFor(projectId);
}