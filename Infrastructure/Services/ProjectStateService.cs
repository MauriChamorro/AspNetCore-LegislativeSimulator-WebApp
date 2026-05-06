using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Infrastructure.Interfaces;

namespace WebAppMVC.Infrastructure.Services;

public class ProjectStateService: IProjectStateService
{
    public bool CanEdit(ProjectState state) => 
        state.CurrentState ==  FileState.Scratch;

    public string GetNameState(FileState fileState) =>
        fileState switch
        {
            FileState.Scratch => "Borrador",
            FileState.InPendingCommissions => "En Asignación de Comisiones",
            FileState.InCommission => "Pendiente de Giros",
            _ => ""
        };

    public ProjectState EmptyProject() =>
        new()
        {
            CurrentState = FileState.Scratch,
            ChangeDate = DateTime.Now
        };
}