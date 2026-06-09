using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.ViewModels;

public class EditProjectAdminVm
{
    public int ProjectId { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string StateName { get; set; }
    public FileState EnumState { get; set; }
}