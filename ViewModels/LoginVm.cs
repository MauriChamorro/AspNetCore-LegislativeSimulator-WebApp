using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.ViewModels;

public class LoginVm
{
    [Required(ErrorMessage = "El campo Usuario es obligatorio")]
    [Display(Name = "Usuario")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "El campo Contraseña es obligatorio")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; }
    
    [Display(Name = "Mantener sesión")]
    public bool RememberMe { get; set; }
}