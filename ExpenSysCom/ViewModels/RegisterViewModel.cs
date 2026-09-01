using System.ComponentModel.DataAnnotations;

namespace ExpenSysCom.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres de largo.", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El número de documento es obligatorio.")]
        [Display(Name = "Número de Documento")]
        [RegularExpression(@"^\d{7,10}$", ErrorMessage = "El número de documento debe tener entre 7 y 10 dígitos")] 
        public string NoDocumento { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Teléfono debe tener exactamente 10 dígitos numéricos.")]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }
    }
}
