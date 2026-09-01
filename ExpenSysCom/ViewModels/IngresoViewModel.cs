using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        [Column(TypeName = "decimal(18, 2)")] // Asegura el tipo de dato correcto
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [Display(Name = "Categoría del Ingreso")]
        public int IdCategoriaIngreso { get; set; }

        // Propiedad para la lista de categorías en el dropdown
        // Se carga en el controlador y se usa en la vista.
        public IEnumerable<SelectListItem>? Categorias { get; set; }
    }
}
