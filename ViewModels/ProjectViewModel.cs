using System.ComponentModel.DataAnnotations;
using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.ViewModels;

public class ProjectViewModel
{
    public int ProjectId { get; set; }
    
    [Required(ErrorMessage = "El campo Título es obligatorio")] 
    [MinLength(10, ErrorMessage = "Debe tener mínimo 10 caracteres")]
    [Display(Name = "Título")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El campo Artículo es obligatorio")]
    [Display(Name = "Artículos")]

    public string Articles { get; set; } = string.Empty;
    [Required(ErrorMessage = "El campo Fundamentos es obligatorio")] [Display(Name = "Fundamentos")]
    
    public string Fundaments { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El campo Resumen es obligatorio")] [Display(Name = "Resumen")]
    public string Summary { get; set; } = string.Empty;
    
    public string StateName { get; set; }
    public DateTime StateDate { get; set; }
}