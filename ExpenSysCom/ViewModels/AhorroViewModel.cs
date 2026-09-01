using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ExpenSysCom.Modelos;
using ExpenSysCom.Data;
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

        [Required(ErrorMessage = "La intensidad es obligatoria.")]
        public int IdIntensidad { get; set; }

        [Required]
        public int IdEstado { get; set; }


        // Nombres de referencia para mostrar en detalles
        public string NombreIntensidad { get; set; } = ""; 
        public string NombreEstado { get; set; } = "";


        // SelectLists para los dropdowns
        public SelectList? Intensidades { get; set; }
        public SelectList? Estados { get; set; }


        // 🔹 Constructor vacío
        public AhorroViewModel() { }


        // 🔹 Constructor para cargar SelectLists desde la DB
        public AhorroViewModel(ExpenSysDBContext context)
        {
            // Cargamos intensidades y estados desde la BD
            Intensidades = new SelectList(context.Intensidades.OrderBy(i => i.Nombre), "IdIntensidad", "Nombre");
            Estados = new SelectList(context.Estados.OrderBy(e => e.Nombre), "IdEstado", "Nombre");
        }


        // 🔹 Método auxiliar para recargar SelectLists en POST si falla ModelState
        public void CargarSelectLists(ExpenSysDBContext context)
        {
            Intensidades = new SelectList(context.Intensidades.OrderBy(i => i.Nombre), "IdIntensidad", "Nombre", IdIntensidad);
            Estados = new SelectList(context.Estados.OrderBy(e => e.Nombre), "IdEstado", "Nombre", IdEstado);
        }
    }
}
