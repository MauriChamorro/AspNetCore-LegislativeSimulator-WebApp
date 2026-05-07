using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Infrastructure.Interfaces;

namespace WebAppMVC.Infrastructure.Services;

public class ProjectStateService: IProjectStateService
{
   

    public string GetNameState(FileState fileState) =>
        fileState switch
        {
            FileState.Scratch => "Borrador",
            FileState.PendingForAssignCommissions => "Asignando de Comisiones",
            FileState.InCommission => "En Comisiones",
            FileState.RejectedByCommissions => "Rechazado por Comisiones",
            _ => ""
        };

    public ProjectState EmptyProject() =>
        new()
        {
            CurrentState = FileState.Scratch,
            ChangeDate = DateTime.Now
        };
    
    public bool CanEdit(ProjectState state) => 
        state.CurrentState ==  FileState.Scratch;
    
    public bool CommissionsAssigned(ProjectState state) => 
            state.CurrentState ==  FileState.InCommission;
}