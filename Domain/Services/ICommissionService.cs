using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Services;

public interface ICommissionService
{
    List<Commission> EvaluateCommissionFor(string projectArticles);
}