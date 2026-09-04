using ExpenSysCom.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ExpenSysCom.ViewModels
{
    public class IngresoViewModel
    {
        public int IdIngreso { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        [Display(Name = "Nombre del Ingreso")]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Descripción (Opcional)")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El valor es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El valor debe ser mayor que 0.")]
        [Column(TypeName = "decimal(18, 2)")] 
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }


        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [Display(Name = "Categoría del Ingreso")]
        public CategoriaIngreso CategoriaIngreso { get; set; }
    }
}
