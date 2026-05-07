using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Interfaces;

public interface ICommissionsVmService
{
    ProjectReferralCommissionsViewModel CreateReferralCommissionsVMs(Project project,
        List<ReferralCommission> referralCommissions);
}