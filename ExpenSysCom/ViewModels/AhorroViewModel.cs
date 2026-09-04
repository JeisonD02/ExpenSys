using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ExpenSysCom.Data;
using ExpenSysCom.Enums;
using ExpenSysCom.Models;
using System.Collections.Generic;
using System.Linq;

namespace ExpenSysCom.ViewModels
{
    public class AhorroViewModel
    {
        public int IdAhorro { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string Nombre { get; set; }


        [StringLength(200, ErrorMessage = "La descripción no puede exceder los 200 caracteres.")]
        public string? Descripcion { get; set; }


        [Required(ErrorMessage = "El porcentaje es obligatorio.")]
        [Range(1, 100, ErrorMessage = "El porcentaje debe estar entre 1 y 100.")]
        public int Porcentaje { get; set; }


        public Intensidad Intensidad { get; set; }
        public Estado Estado { get; set; }


        public AhorroViewModel() { }

    }
}
