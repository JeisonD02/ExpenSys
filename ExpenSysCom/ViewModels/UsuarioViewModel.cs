

using System.ComponentModel.DataAnnotations;

namespace ExpenSysCom.ViewModels
{
    public class UsuarioViewModel
    {
        public string? Id { get; set;}


        [Required(ErrorMessage = "El Nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string UserName { get; set;}


        [Required(ErrorMessage = "El Correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "El Número de Documento es obligatorio.")]
        [RegularExpression(@"^\d{7,10}$", ErrorMessage = "El número de documento debe tener entre 7 y 10 dígitos")] 
        public string NoDocumento { get; set; }


        [RegularExpression(@"^\d{10}$", ErrorMessage = "Teléfono debe tener exactamente 10 dígitos numéricos.")] 
        public string? PhoneNumber { get; set; }


        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres de largo.", MinimumLength = 6)]
        public string? Password { get; set; }


        [Required(ErrorMessage = "El Rol es obligatorio.")]
        public string Role { get; set; }


        public bool IsActive { get; set; } = true;
        }
}
