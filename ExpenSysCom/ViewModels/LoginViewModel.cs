using System.ComponentModel.DataAnnotations;

namespace ExpenSysCom.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo electrónico o nombre de usuario es obligatorio.")]
        [Display(Name = "Correo o Nombre de Usuario")]
        public string LoginIdentifier { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }
}
