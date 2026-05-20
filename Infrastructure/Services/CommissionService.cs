using System.Text.RegularExpressions;
using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Domain.Services;

namespace WebAppMVC.Infrastructure.Services;

public class CommissionService : ICommissionService
{
    private readonly IReferralCommissionRepository _referralCommissionRepository;
    private readonly IProjectRepository _projectRepository;


    public CommissionService(IReferralCommissionRepository referralCommissionRepository,
        IProjectRepository projectRepository)
    {
        _referralCommissionRepository = referralCommissionRepository;
        _projectRepository = projectRepository;
    }

    public List<Commission> EvaluateCommissionFor(string projectArticles)
    {
        var assignedCommissions = new List<Commission>();
        var words = new List<string>();
        foreach (var commission in _projectRepository.GetCommissions())
        {
            if (commission.WordsForAssignment != null)
                foreach (var commissionWord in commission.WordsForAssignment)
                {
                    if (Regex.IsMatch(projectArticles, $@"\b{commissionWord}\b", RegexOptions.IgnoreCase))
                    {
                        words.Add(commissionWord);
                        assignedCommissions.Add(commission);
                        break;
                    }
                }
        }

        foreach (var word in words)
            Console.Write(word);
        return assignedCommissions;
    }

    public List<Referral> AssignCommissionTo(List<Commission> commissions, int projectId)
    {
        var referralCommissions = new List<Referral>();
        foreach (var commission in commissions)
        {
            referralCommissions.Add(
                new Referral
                {
                    ProjectId = projectId,
                    CommissionId = commission.CommissionId,
                    State = ReferralCommissionState.Assigned,
                    Date = DateTime.Now
                }
            );
        }
        _projectRepository.AddReferrals(referralCommissions);
        return referralCommissions;
    }

    public bool HasBeenAssigned(int projectId) =>
        _referralCommissionRepository.ExistProjectId(projectId);

    public List<Referral> GetReferralCommissionsFor(int projectId) => 
        _referralCommissionRepository.GetFor(projectId);

    public Referral GetActualReferral(List<Referral> referralCommissions)
    {
        if (referralCommissions.Any(rc => rc.State == ReferralCommissionState.Evaluating))
            return referralCommissions.First(rc => rc.State == ReferralCommissionState.Evaluating);
        return referralCommissions.First(rc => rc.State == ReferralCommissionState.Assigned);
    }

    public void DoNextReferralPhase(Referral actualReferral)
    {
        if (actualReferral.State == ReferralCommissionState.Assigned)
        {
            actualReferral.State = ReferralCommissionState.Evaluating;
            actualReferral.Date = DateTime.Now;
        }
        else if (actualReferral.State == ReferralCommissionState.Evaluating)
        {
            actualReferral.State = GetRandomResult();
            actualReferral.Date = DateTime.Now;
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

    public bool ThereAreNotPendingReferral(List<Referral> referralCommissions) =>
        referralCommissions.TrueForAll(rc =>
            rc.State == ReferralCommissionState.Accepted || rc.State == ReferralCommissionState.Rejected);

    public bool ReferralIsRejected(Referral actualReferral) =>
        actualReferral.State == ReferralCommissionState.Rejected;

    public bool AcceptedByAllCommission(int projectId) =>
        _referralCommissionRepository.GetFor(projectId)
            .TrueForAll(rc => rc.State == ReferralCommissionState.Accepted);
}