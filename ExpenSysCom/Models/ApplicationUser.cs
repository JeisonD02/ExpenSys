using Microsoft.AspNetCore.Identity;

namespace ExpenSysCom.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NoDocumento { get; set; }

        public bool IsActive { get; set; } = true;
    }
}