using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.ViewModels;

public class CredentialVm
{
    [Required]
    [Display(Name = "Nombre de Usuario")]
    public string Username { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; }
}