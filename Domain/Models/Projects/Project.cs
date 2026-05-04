using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Domain.Models.Projects;

public class Project
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El campo Título es obligatorio")] 
    [MinLength(10, ErrorMessage = "Debe tener mínimo 10 caracteres")]
    [Display(Name = "Título")]
    public string? Title { get; set; }
    [Required(ErrorMessage = "El campo Artículo es obligatorio")] [Display(Name = "Artículos")]
    public string? Articles { get; set; }
    [Required(ErrorMessage = "El campo Fundamentos es obligatorio")] [Display(Name = "Fundamentos")]
    public string? Fundaments { get; set; }
    [Required(ErrorMessage = "El campo Resumen es obligatorio")] [Display(Name = "Resumen")]
    public string? Summary { get; set; }
    public int ProjectStateId { get; set; }
    [Required]
    public ProjectState State { get; set; }
}