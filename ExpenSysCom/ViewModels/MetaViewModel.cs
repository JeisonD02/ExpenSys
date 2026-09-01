using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ExpenSysCom.Modelos;

namespace ExpenSysCom.ViewModels;

public class MetaViewModel
{
    public int IdMeta { get; set; }

    [Required(ErrorMessage = "El nombre de la meta es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    [DisplayName("Nombre de la Meta")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
    [DisplayName("Descripción")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El valor es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El valor debe ser mayor a 0.")]
    [DisplayName("Valor de la Meta")]
    [DataType(DataType.Currency)]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
    [DisplayName("Fecha de Inicio")]
    [DataType(DataType.Date)]
    public DateOnly FechaInicio { get; set; }

    [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
    [DisplayName("Fecha de Fin")]
    [DataType(DataType.Date)]
    public DateOnly FechaFin { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un Ahorro.")]
    [DisplayName("Ahorro")]
    public int IdAhorro { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un Estado.")]
    [DisplayName("Estado")]
    public int IdEstado { get; set; }

    //Proppiedades para Details y Delete
    public string? NombreAhorro { get; set; }  // <-- Nuevo
    public string? NombreEstado { get; set; }  // <-- Nuevo


    // Propiedades para los dropdowns de las vistas de "Crear" y "Editar"
    // Estas propiedades no son parte del modelo de la base de datos
    public SelectList? Ahorros { get; set; }
    public SelectList? Estados { get; set; }
}
