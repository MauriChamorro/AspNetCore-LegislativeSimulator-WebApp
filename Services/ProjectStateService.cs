using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Services.Interfaces;

namespace WebAppMVC.Services;

public class ProjectStateService: IProjectStateService
{
    public bool CanEdit(ProjectState state) => 
        state.CurrentState ==  FileState.Scratch;

    public string GetNameState(FileState fileState) =>
        fileState switch
        {
            FileState.Scratch => "Borrador",
            FileState.InPendingCommissions => "En Asignación de Comisiones",
            _ => ""
        };

    public ProjectState EmptyProject() =>
        new()
        {
            CurrentState = FileState.Scratch,
            ChangeDate = DateTime.Now
        };
}