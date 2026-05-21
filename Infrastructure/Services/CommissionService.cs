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
        foreach (var commission in _projectRepository.GetCommissions())
        {
            if (commission.WordsForAssignment != null)
                foreach (var commissionWord in commission.WordsForAssignment)
                {
                    if (Regex.IsMatch(projectArticles, $@"\b{commissionWord}\b", RegexOptions.IgnoreCase))
                    {
                        assignedCommissions.Add(commission);
                        break;
                    }
                }
        }
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
                    State = ReferralState.Assigned,
                    DateState = DateTime.Now
                }
            );
        }
        _projectRepository.AddReferrals(referralCommissions);
        return referralCommissions;
    }

    public bool HasBeenAssigned(int projectId) => 
        _projectRepository.HasReferrals(projectId);

    public List<Referral> GetReferralsFor(int projectId) => 
        _projectRepository.GetReferralsFor(projectId);

    public Referral GetActualReferralFor(List<Referral> referrals)
    {
        if (referrals.Any(rc => rc.State == ReferralState.Evaluating))
            return referrals.First(rc => rc.State == ReferralState.Evaluating);
        return referrals.First(rc => rc.State == ReferralState.Assigned);
    }

    public void DoNextReferralPhase(Referral actualReferral)
    {
        if (actualReferral.State == ReferralState.Assigned)
        {
            actualReferral.State = ReferralState.Evaluating;
            actualReferral.DateState = DateTime.Now;
        }
        else if (actualReferral.State == ReferralState.Evaluating)
        {
            actualReferral.State = GetRandomResult();
            actualReferral.DateState = DateTime.Now;
        }
        
        _projectRepository.UpdateReferral(actualReferral);
    }

    private static ReferralState GetRandomResult()
    {
        var rnd = new Random();
        var success = rnd.Next(2) == 0;
        if (success)
            return ReferralState.Accepted;
        return ReferralState.Rejected;
    }

    public bool AllCommissionEvaluated(List<Referral> referrals) =>
        referrals.TrueForAll(rc =>
            rc.State == ReferralState.Accepted || rc.State == ReferralState.Rejected);

    public bool IsRejectedReferral(Referral actualReferral) =>
        actualReferral.State == ReferralState.Rejected;

    public bool AcceptedByAllCommission(List<Referral> referrals) =>
        referrals.TrueForAll(rc => rc.State == ReferralState.Accepted);

    public bool AlreadyRejected(List<Referral> referrals)=>
        referrals.Any(rc => rc.State == ReferralState.Rejected);
}