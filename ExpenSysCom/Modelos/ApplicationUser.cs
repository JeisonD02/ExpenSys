using Microsoft.AspNetCore.Identity;

namespace ExpenSysCom.Modelos
{
    // Extiende la clase IdentityUser para añadir propiedades personalizadas.
    public class ApplicationUser : IdentityUser
    {
        public string NoDocumento { get; set; }

        // Nuevo campo para activar/desactivar
        public bool IsActive { get; set; } = true;
    }
}